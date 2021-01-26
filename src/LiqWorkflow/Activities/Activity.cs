using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Models;
using LiqWorkflow.Abstractions.Models.Configurations;

namespace LiqWorkflow.Activities
{
    public class Activity : IWorkflowActivity
    {
        private readonly IWorkflowActivity _activity;

        public Activity(IWorkflowActivity activity)
        {
            _activity = activity;

            Configuration = activity.Configuration;
            Branches = activity.Branches;
        }

        public ActivityConfiguration Configuration { get; }

        public ImmutableDictionary<string, IWorkflowBranch> Branches { get; }

        public async Task<WorkflowResult<ActivityData>> ExecuteAsync(ActivityData data, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await _activity.ExecuteAsync(data, cancellationToken);

                return result;
            }
            catch (OperationCanceledException exception)
            {
                return ProcessErrorResult(exception);
            }
            catch (Exception exception)
            {
                return ProcessErrorResult(exception);
            }
        }

        private WorkflowResult<ActivityData> ProcessErrorResult(Exception exception)
        {


            return WorkflowResult<ActivityData>.Error(exception);
        }
    }
}
