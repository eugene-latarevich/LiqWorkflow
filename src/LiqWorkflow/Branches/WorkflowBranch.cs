using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;
using LiqWorkflow.Abstractions.Events;
using LiqWorkflow.Abstractions.Models;
using LiqWorkflow.Activities;
using LiqWorkflow.Common.Extensions;
using LiqWorkflow.Common.Helpers;
using LiqWorkflow.Exceptions;

namespace LiqWorkflow.Branches
{
    class WorkflowBranch : IWorkflowBranch, IWorkflowBranchContinuation
    {
        private readonly IWorkflowConfiguration _workflowConfiguration;
        private readonly IWorkflowMessageEventBroker _workflowMessageEventBroker;

        public WorkflowBranch(
            IBranchConfiguration branchConfiguration,
            IWorkflowConfiguration workflowConfiguration,
            IDictionary<string, IWorkflowActivity> activities)
        {
            _workflowConfiguration = workflowConfiguration;
            _workflowMessageEventBroker = workflowConfiguration.Services.GetService<IWorkflowMessageEventBroker>();

            Configuration = branchConfiguration;
            Activities = new OrderedActivityCollection(activities);
        }

        public IBranchConfiguration Configuration { get; }

        public IOrderedActivityCollection Activities { get; }

        public Task PulseAsync(CancellationToken cancellationToken) => ProcessActivitiesAsync(default, Activities, cancellationToken);
        
        public async Task RestoreAsync(CancellationToken cancellationToken)
        {
            foreach (var activity in Activities)
            {
                if (activity.Configuration.RestorePoint)
                {
                    await ProcessActivitiesAsync(activity.Configuration.Create(), Activities.Clone(), cancellationToken)
                        .ConfigureAwait(false);
                }
                else
                {
                    await activity.Branches
                        .Select(x => x.Value)
                        .ForEachAsync(branch => branch.RestoreAsync(cancellationToken))
                        .ConfigureAwait(false);
                }
            }
        }

        public Task ContinueWithAsync(ActivityData initialData, CancellationToken cancellationToken) => ProcessActivitiesAsync(initialData, Activities.Clone(), cancellationToken);

        public bool IsValid() => Activities.ValidateBranchStartEnd() && Activities.ValidateInnerBranches();

        private async Task ProcessActivitiesAsync(ActivityData initialData, IOrderedActivityCollection activities, CancellationToken cancellationToken)
        {
            ActivityData lastResult = initialData;
            foreach (var activity in activities.StartFrom(initialData.GetStartFromActivityId()))
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var activityResult = await TaskHelper.RetryOnConditionOrException(
                            condition: result => result.Succeeded,
                            retryFunc: () => ExecuteAsync(lastResult, activity, cancellationToken),
                            retryCount: _workflowConfiguration.RetrySetting.RetryCount,
                            delay: _workflowConfiguration.RetrySetting.Delay,
                            cancellationToken)
                        .ConfigureAwait(false);

                    lastResult = activityResult.Value;
                }
                catch (OperationCanceledException exception)
                {
                    ProcessError(activity, exception);
                    return;
                }
                catch (Exception exception)
                {
                    var message = ProcessError(activity, exception);
                    throw new Exception(message, exception);
                }
            }
        }

        private Task<WorkflowResult<ActivityData>> ExecuteAsync(ActivityData initialData, IWorkflowExecutableActivity activity, CancellationToken cancellationToken)
        {
            if (activity.Configuration.RestorePoint)
            {
                if (activity is IRestorableWorkflowActivitity restorableActivity)
                {
                    return restorableActivity.ExecuteAsync(initialData, cancellationToken);
                }

                throw new UnknownTypeException($"Restorable activity must implement {typeof(IRestorableWorkflowActivitity)} type.");
            }

            return activity.ExecuteAsync(initialData, cancellationToken);
        }

        private string ProcessError(IWorkflowActivity activity, Exception exception)
        {
            var message = $"Error on executing Activity with Id={activity.Configuration.ActivityId}";
            _workflowMessageEventBroker.PublishMessage(OnLogData.Error(message, exception));
            return message;
        }
    }
}
