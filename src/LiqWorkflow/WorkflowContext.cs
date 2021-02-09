using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Containers;
using LiqWorkflow.Abstractions.Events;
using LiqWorkflow.Abstractions.Models;
using LiqWorkflow.Common.Extensions;
using LiqWorkflow.Exceptions;

namespace LiqWorkflow
{
    public abstract class WorkflowContext : IWorkflowExecutableContext, IDisposable
    {
        private readonly IContainer _container;
        private readonly IWorkflowMessageEventBroker _workflowMessageEventBroker;
        private readonly ConcurrentDictionary<string, IWorkflow> _workflows = new ConcurrentDictionary<string, IWorkflow>();
        private readonly IDisposable _eventBrokerUnsubscriber;

        protected WorkflowContext(
            IContainer container,
            IWorkflowConfiguration workflowConfiguration,
            IWorkflowMessageEventBroker workflowMessageEventBroker)
        {
            _container = container;
            _workflowMessageEventBroker = workflowMessageEventBroker;

            _eventBrokerUnsubscriber = _workflowMessageEventBroker.Subscribe(
                onNext: async args => await OnLogAsync(args, workflowConfiguration.CancellationTokenSource.Token));
        }

        public bool TryRemoveWorkflow(string workflowId) => _workflows.TryRemove(workflowId, out _);

        public virtual bool TryAddWorkflow(IWorkflowInitData workflowInitData)
        {
            var workflowBuilder = _container.GetService<IWorkflowBuilder>();
            var workflow = workflowBuilder.BuildWorkflow(workflowInitData);

            return TryAddWorkflow(workflowInitData.WorkflowConfiguration.Id, workflow);
        }

        public Task<WorkflowResult> StartWorkflowAsync(string workflowId) 
            => _workflows.TryGetValue(workflowId, out var workflow)
                ? workflow.StartAsync()
                : WorkflowResult.Error(new NotFoundException($"Workflow with WorkflowId={workflowId} wasn't found.")).AsTask();

        public Task<WorkflowResult> RestoreWorkflowAsync(string workflowId) 
            => _workflows.TryGetValue(workflowId, out var workflow)
                ? workflow.RestoreAsync()
                : WorkflowResult.Error(new NotFoundException($"Workflow with WorkflowId={workflowId} wasn't found.")).AsTask();

        public Task<WorkflowResult> StopWorkflowAsync(string workflowId) 
            => _workflows.TryGetValue(workflowId, out var workflow)
                ? workflow.StopAsync()
                : WorkflowResult.Error(new NotFoundException($"Workflow with WorkflowId={workflowId} wasn't found.")).AsTask();

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

        protected bool TryAddWorkflow(string workflowId, IWorkflow workflow) => _workflows.TryAdd(workflowId, workflow);

        protected abstract Task OnActivityResultAsync(ActivityData result, CancellationToken cancellation);

        protected abstract Task OnLogAsync(OnLogData logData, CancellationToken cancellationToken);
    }
}
