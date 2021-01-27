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
using Microsoft.Extensions.Logging;

namespace LiqWorkflow
{
    // TODO
    // get and store initial data
    // get and store results
    // send data to context
    public class WorkflowBranch : IWorkflowBranch, IWorkflowBranchContinuation
    {
        private readonly IWorkflowConfiguration _workflowConfiguration;
        private readonly ILogger<WorkflowBranch> _logger;

        public WorkflowBranch(
            IWorkflowConfiguration workflowConfiguration,
            ImmutableDictionary<string, IWorkflowActivity> activities,
            ILogger<WorkflowBranch> logger)
        {
            _workflowConfiguration = workflowConfiguration;
            _logger = logger;

            Activities = new OrderedActivityCollection(activities);
        }

        public IOrderedActivityCollection Activities { get; }

        public async Task PulseAsync(CancellationToken cancellationToken)
        {
            ActivityData lastResult = null;
            foreach (var activity in Activities)
            {
                try
                {
                    var activityData = MergeInitialData(lastResult);
                    var activityResult = await TaskHelper.RetryOnConditionOrException(
                        condition: result => result.Succeeded,
                        retryFunc: () => activity.ExecuteAsync(activityData, cancellationToken),
                        retryCount: _workflowConfiguration.RetrySetting.RetryCount,
                        delay: _workflowConfiguration.RetrySetting.Delay,
                        cancellationToken);

                    lastResult = activityResult.Value;
                }
                catch (Exception exception)
                {
                    var message = $"Error on executing Activity with Id={activity.Configuration.ActivityId}";
                    //todo log through context
                    _logger.LogError(exception, message);
                    throw new Exception(message, exception);
                }
            }
        }

        public async Task ContinueWithAsync(ActivityData result, CancellationToken cancellationToken)
        {
            
        }

        public bool IsValid() => Activities.ValidateBranchStartEnd() && Activities.ValidateInnerBranches();

        private ActivityData MergeInitialData(ActivityData activityData)
        {
            return activityData;
        }
    }
}
