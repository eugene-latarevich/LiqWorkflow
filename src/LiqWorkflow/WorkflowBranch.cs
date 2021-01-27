using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Models;
using LiqWorkflow.Activities;
using LiqWorkflow.Common.Extensions;
using LiqWorkflow.Common.Helpers;

namespace LiqWorkflow
{
    // TODO
    // send data to context
    public class WorkflowBranch : IWorkflowBranch, IWorkflowBranchContinuation
    {
        private readonly IWorkflowConfiguration _workflowConfiguration;

        public WorkflowBranch(
            IWorkflowConfiguration workflowConfiguration,
            ImmutableDictionary<string, IWorkflowActivity> activities)
        {
            _workflowConfiguration = workflowConfiguration;

            Activities = new OrderedActivityCollection(activities);
        }

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
                    var activityResult = await TaskHelper.RetryOnConditionOrException(
                        condition: result => result.Succeeded,
                        retryFunc: () => activity.ExecuteAsync(lastResult, cancellationToken),
                        retryCount: _workflowConfiguration.RetrySetting.RetryCount,
                        delay: _workflowConfiguration.RetrySetting.Delay,
                        cancellationToken);

                    lastResult = activityResult.Value;
                }
                catch (Exception exception)
                {
                    var message = $"Error on executing Activity with Id={activity.Configuration.ActivityId}";
                    //todo log through context
                    //_logger.LogError(exception, message);
                    throw new Exception(message, exception);
                }
            }
        }
    }
}
