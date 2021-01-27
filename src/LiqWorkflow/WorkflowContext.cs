using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Events;
using LiqWorkflow.Abstractions.Models;
using LiqWorkflow.Common.Extensions;

namespace LiqWorkflow
{
    public abstract class WorkflowContext : IDisposable
    {
        private readonly IDisposable _eventBrokerUnsubscriber;
        private readonly IWorkflowMessageEventBroker _workflowMessageEventBroker;
        private readonly ConcurrentDictionary<string, IWorkflow> _workflows = new ConcurrentDictionary<string, IWorkflow>();

        protected WorkflowContext(
            IWorkflowConfiguration workflowConfiguration,
            IWorkflowMessageEventBroker workflowMessageEventBroker)
        {
            _workflowMessageEventBroker = workflowMessageEventBroker;

            _eventBrokerUnsubscriber = _workflowMessageEventBroker.Subscribe(
                onNext: async args => await OnLogAsync(args, workflowConfiguration.CancellationTokenSource.Token));
        }

        public bool TryAddWorkflow(string workflowId, IWorkflow workflow) => _workflows.TryAdd(workflowId, workflow);

        public bool TryRemoveWorkflow(string workflowId) => _workflows.TryRemove(workflowId, out _);

        public virtual void Dispose()
        {
            _eventBrokerUnsubscriber.Dispose();

            _workflows
                .Select(x => x.Value)
                .ForEach(workflow =>
                {
                    if (workflow is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                });
        }

        protected abstract Task OnActivityResultAsync(ActivityData result, CancellationToken cancellation);

        protected abstract Task OnLogAsync(OnLogData logData, CancellationToken cancellationToken);
    }
}
