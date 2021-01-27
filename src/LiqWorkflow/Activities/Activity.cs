using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Events;
using LiqWorkflow.Abstractions.Models;
using LiqWorkflow.Abstractions.Models.Configurations;
using LiqWorkflow.Common.Extensions;

namespace LiqWorkflow.Activities
{
    public abstract class Activity : IWorkflowActivity
    {
        private readonly IWorkflowActivity _activity;
        private readonly IWorkflowMessageEventBroker _workflowMessageEventBroker;

        protected Activity(
            IWorkflowActivity activity,
            IWorkflowMessageEventBroker workflowMessageEventBroker)
        {
            _activity = activity;
            _workflowMessageEventBroker = workflowMessageEventBroker;

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

                var result = await GetAndProcessResultAsync(processingData, cancellationToken);

                return result.Succeeded ? result : ProcessErrorResult(result.Exception);
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

        protected abstract Task ProcessResultAsync(WorkflowResult<ActivityData> result, CancellationToken cancellationToken);

        protected abstract Task<ActivityData> LoadInitialDataAsync(CancellationToken cancellationToken);

        protected abstract ActivityData MergeInitialData(ActivityData initialData, ActivityData data);

        private async Task<WorkflowResult<ActivityData>> GetAndProcessResultAsync(ActivityData data, CancellationToken cancellationToken)
        {
            MessageOnStartActivity(data);
            var result = await _activity.ExecuteAsync(data, cancellationToken);

            await SendToConnectedBranchesAsync(result, cancellationToken);

            await ProcessResultAsync(result, cancellationToken);
            MessageOnFinishActivity(result);

            return result;
        }

        private async Task SendToConnectedBranchesAsync(WorkflowResult<ActivityData> result, CancellationToken cancellationToken)
        {
            if (result.Succeeded && Branches != null)
            {
                await Branches
                    .Select(x => x.Value)
                    .ForEachAsync(async branch =>
                    {
                        if (branch is IWorkflowBranchContinuation continuation)
                        {
                            await continuation.ContinueWithAsync(result.Value, cancellationToken);
                        }
                    })
                    .ConfigureAwait(false);
            }
        }

        private void MessageOnStartActivity(ActivityData data)
        {
            var message = $"Activity with Id={Configuration.ActivityId} has been started.";
            _workflowMessageEventBroker.PublishMessage(OnLogData.Info(message, data));
        }

        private void MessageOnFinishActivity(WorkflowResult<ActivityData> result)
        {
            if (result.Succeeded)
            {
                var message = $"Activity with Id={Configuration.ActivityId} has been finished.";
                _workflowMessageEventBroker.PublishMessage(OnLogData.Info(message, result.Value));
            }
        }

        private WorkflowResult<ActivityData> ProcessErrorResult(Exception exception)
        {
            var message = $"Error on executing Activity with Id={Configuration.ActivityId}";
            _workflowMessageEventBroker.PublishMessage(OnLogData.Error(message, exception));
            return WorkflowResult<ActivityData>.Error(exception);
        }
    }
}
