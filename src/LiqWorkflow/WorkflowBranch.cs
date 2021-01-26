using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Models;
using LiqWorkflow.Common.Extensions;
using LiqWorkflow.Common.Helpers;
using Microsoft.Extensions.Logging;

namespace LiqWorkflow
{
    // TODO
    // get and store initial data
    // get and store results
    public class WorkflowBranch : IWorkflowBranch
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

            Activities = activities;
        }

        public ImmutableDictionary<string, IWorkflowActivity> Activities { get; }

        public async Task PulseAsync(CancellationToken cancellationToken)
        {
            ActivityData lastResult = null;
            foreach (var activity in Activities.Select(x => x.Value))
            {
                try
                {
                    var activityData = MergeInitialData(lastResult);
                    var activityResult = await TaskHelper.RetryOnConditionOrException(
                        condition: result => result.Succeeded,
                        retryFunc: () => activity.ExecuteAsync(activityData, cancellationToken),
                        _workflowConfiguration.RetrySetting.RetryCount,
                        _workflowConfiguration.RetrySetting.Delay,
                        cancellationToken);

                    lastResult = activityResult.Value;
                }
                catch (Exception exception)
                {
                    var message = $"Error on executin Activity with Id={activity.Configuration.ActivityId}";
                    _logger.LogError(exception, message);
                    throw new Exception(message, exception);
                }
            }
        }

        public bool IsValid()
        {
            var activities = Activities.Select(x => x.Value);
            return activities.ValidateBranchStartEnd() && activities.ValidateInnerBranches();
        }

        private ActivityData MergeInitialData(ActivityData activityData)
        {
            return activityData;
        }
    }
}
