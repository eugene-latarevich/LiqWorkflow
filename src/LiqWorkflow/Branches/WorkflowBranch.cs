using System;
using System.Collections.Generic;
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
using Microsoft.Extensions.DependencyInjection;

namespace LiqWorkflow.Branches
{
    public class WorkflowBranch : IWorkflowBranch, IWorkflowBranchContinuation
    {
        private readonly IWorkflowConfiguration _workflowConfiguration;
        private readonly IWorkflowMessageEventBroker _workflowMessageEventBroker;

        public WorkflowBranch(
            IBranchConfiguration branchConfiguration,
            IWorkflowConfiguration workflowConfiguration,
            IDictionary<string, IWorkflowActivity> activities)
        {
            _workflowConfiguration = workflowConfiguration;
            _workflowMessageEventBroker = workflowConfiguration.ServiceProvider.GetService<IWorkflowMessageEventBroker>();

            Configuration = branchConfiguration;
            Activities = new OrderedActivityCollection(activities);
        }

        public IBranchConfiguration Configuration { get; }

        public IOrderedActivityCollection Activities { get; }

        public Task PulseAsync(CancellationToken cancellationToken) => ProcessActivitiesAsync(null, Activities, cancellationToken);

        public Task ContinueWithAsync(ActivityData initialData, CancellationToken cancellationToken)
        {
            var clonedActivities = Activities.Clone().StartFrom(initialData.ActivityToId);
            return ProcessActivitiesAsync(initialData, clonedActivities, cancellationToken);
        }

        public bool IsValid() => Activities.ValidateBranchStartEnd() && Activities.ValidateInnerBranches();

        private async Task ProcessActivitiesAsync(ActivityData initialData, IOrderedActivityCollection activities, CancellationToken cancellationToken)
        {
            ActivityData lastResult = initialData;
            foreach (var activity in activities)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var activityResult = await TaskHelper.RetryOnConditionOrException(
                            condition: result => result.Succeeded,
                            retryFunc: () => activity.ExecuteAsync(lastResult, cancellationToken),
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

        private string ProcessError(IWorkflowActivity activity, Exception exception)
        {
            var message = $"Error on executing Activity with Id={activity.Configuration.ActivityId}";
            _workflowMessageEventBroker.PublishMessage(OnLogData.Error(message, exception));
            return message;
        }
    }
}
