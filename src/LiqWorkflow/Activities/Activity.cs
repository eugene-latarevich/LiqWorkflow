using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;
using LiqWorkflow.Abstractions.Events;
using LiqWorkflow.Abstractions.Models;
using LiqWorkflow.Branches;
using LiqWorkflow.Common.Extensions;

namespace LiqWorkflow.Activities
{
    public abstract class Activity : IWorkflowExecutableActivity
    {
        private readonly IWorkflowExecutableAction _action;
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        protected readonly IWorkflowMessageEventBroker _workflowMessageEventBroker;

        protected Activity(
            IWorkflowExecutableAction action,
            IActivityConfiguration configuration,
            IDictionary<string, IWorkflowBranch> branches)
        {
            _action = action;
            _workflowMessageEventBroker = configuration.Services.GetService<IWorkflowMessageEventBroker>();

            Configuration = configuration;
            Branches = branches.ToImmutableDictionary();
        }

        public IActivityConfiguration Configuration { get; }

        public ImmutableDictionary<string, IWorkflowBranch> Branches { get; }

        public async Task<WorkflowResult<ActivityData>> ExecuteAsync(ActivityData data, CancellationToken cancellationToken = default)
        {
            await _semaphoreSlim.WaitAsync(cancellationToken);
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var initialData = await LoadInitialDataAsync(cancellationToken).ConfigureAwait(false);

                var processingData = MergeInitialData(initialData, data);

                var result = await GetAndProcessResultAsync(processingData, cancellationToken).ConfigureAwait(false);

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
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        protected abstract Task ProcessResultAsync(WorkflowResult<ActivityData> result, CancellationToken cancellationToken);

        protected abstract Task<ActivityData> LoadInitialDataAsync(CancellationToken cancellationToken);

        protected virtual ActivityData MergeInitialData(ActivityData initialData, ActivityData data) => initialData.Map(data);

        protected virtual WorkflowResult<ActivityData> ProcessErrorResult(Exception exception)
        {
            var message = $"Error on executing Activity with Id={Configuration.ActivityId}";
            _workflowMessageEventBroker.PublishMessage(OnLogData.Error(message, exception));
            return WorkflowResult<ActivityData>.Error(exception);
        }

        private async Task<WorkflowResult<ActivityData>> GetAndProcessResultAsync(ActivityData data, CancellationToken cancellationToken)
        {
            MessageOnStartActivity(data);
            var result = await _action.ExecuteAsync(data, cancellationToken).ConfigureAwait(false);

            await SendToConnectedBranchesAsync(result, cancellationToken).ConfigureAwait(false);

            await ProcessResultAsync(result, cancellationToken).ConfigureAwait(false);
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
                        if (branch is IWorkflowBranchContinuation continuationBranch)
                        {
                            await continuationBranch.ContinueWithAsync(result.Value, cancellationToken).ConfigureAwait(false);
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
    }
}
