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
    public abstract class Activity : IWorkflowActivity
    {
        private readonly IWorkflowActivity _activity;

        protected Activity(IWorkflowActivity activity)
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

                var initialData = await LoadInitialDataAsync(cancellationToken);
                
                var processingData = MergeInitialData(initialData, data);

                var result = await _activity.ExecuteAsync(processingData, cancellationToken);

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

        protected abstract Task<ActivityData> LoadInitialDataAsync(CancellationToken cancellationToken);

        protected abstract ActivityData MergeInitialData(ActivityData initialData, ActivityData data);

        private WorkflowResult<ActivityData> ProcessErrorResult(Exception exception)
        {


            return WorkflowResult<ActivityData>.Error(exception);
        }
    }
}
