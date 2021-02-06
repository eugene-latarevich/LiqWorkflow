using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Branches;
using LiqWorkflow.Abstractions.Events;
using LiqWorkflow.Abstractions.Models;
using LiqWorkflow.Abstractions.Models.Enums;
using LiqWorkflow.Common.Extensions;
using LiqWorkflow.Exceptions;

namespace LiqWorkflow
{
    class Workflow : IWorkflow, IDisposable
    {
        private readonly IEnumerable<IWorkflowBranch> _branches;
        private readonly IWorkflowMessageEventBroker _workflowMessageEventBroker;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        public Workflow(
            IWorkflowConfiguration configuration,
            IEnumerable<IWorkflowBranch> branches,
            IWorkflowMessageEventBroker workflowMessageEventBroker)
        {
            Configuration = configuration;

            _branches = branches;
            _workflowMessageEventBroker = workflowMessageEventBroker;
        }

        public WorkflowStatus Status { get; private set; } = WorkflowStatus.NotStarted;

        public IWorkflowConfiguration Configuration { get; }

        public Task<WorkflowResult> StartAsync() => LaunchAsync(branch => branch.PulseAsync(Configuration.CancellationTokenSource.Token));

        public Task<WorkflowResult> RestoreAsync() => LaunchAsync(branch => branch.RestoreAsync(Configuration.CancellationTokenSource.Token));

        public async Task<WorkflowResult> StopAsync()
        {
            await _semaphoreSlim.WaitAsync(Configuration.CancellationTokenSource.Token);
            try
            {
                Status = WorkflowStatus.Stopping;

                Configuration.CancellationTokenSource.Cancel(true);

                Status = WorkflowStatus.Stopped;

                return WorkflowResult.Ok();
            }
            catch (Exception exception)
            {
                var message = $"Error on stop Workflow with Id={Configuration.Id}";
                _workflowMessageEventBroker.PublishMessage(OnLogData.Error(message, exception));
            }
            finally
            {
                _semaphoreSlim.Release();
            }
           
            return WorkflowResult.Error();
        }

        public void Dispose()
        {
            _semaphoreSlim.Dispose();
        }

        private void ThrowIfNotValidConfiguration()
        {
            if (_branches == null || !_branches.Any() || _branches.Any(branch => !branch.IsValid()))
            {
                throw new InvalidWorkflowConfigurationException();
            }
        }

        private async Task<WorkflowResult> LaunchAsync(Func<IWorkflowBranch, Task> onStart)
        {
            await _semaphoreSlim.WaitAsync(Configuration.CancellationTokenSource.Token);
            try
            {
                ThrowIfNotValidConfiguration();

                Status = WorkflowStatus.Starting;

                await _branches.ForEachAsync(onStart).ConfigureAwait(false);

                Status = WorkflowStatus.Executing;

                return WorkflowResult.Ok();
            }
            catch (Exception exception)
            {
                var message = $"Error on start Workflow with Id={Configuration.Id}";
                _workflowMessageEventBroker.PublishMessage(OnLogData.Error(message, exception));
            }
            finally
            {
                _semaphoreSlim.Release();
            }

            return WorkflowResult.Error();
        }
    }
}
