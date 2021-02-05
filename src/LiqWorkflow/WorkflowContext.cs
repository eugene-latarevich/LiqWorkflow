using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Events;
using LiqWorkflow.Abstractions.Models;
using LiqWorkflow.Common.Extensions;
using LiqWorkflow.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace LiqWorkflow
{
    public abstract class WorkflowContext : IWorkflowExecutableContext, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IWorkflowMessageEventBroker _workflowMessageEventBroker;
        private readonly ConcurrentDictionary<string, IWorkflow> _workflows = new ConcurrentDictionary<string, IWorkflow>();
        private readonly IDisposable _eventBrokerUnsubscriber;

        protected WorkflowContext(
            IServiceProvider serviceProvider,
            IWorkflowConfiguration workflowConfiguration,
            IWorkflowMessageEventBroker workflowMessageEventBroker)
        {
            _serviceProvider = serviceProvider;
            _workflowMessageEventBroker = workflowMessageEventBroker;

            _eventBrokerUnsubscriber = _workflowMessageEventBroker.Subscribe(
                onNext: async args => await OnLogAsync(args, workflowConfiguration.CancellationTokenSource.Token));
        }

        public bool TryRemoveWorkflow(string workflowId) => _workflows.TryRemove(workflowId, out _);

        public virtual void TryAddWorkflow(IWorkflowInitData workflowInitData)
        {
            var workflow = BuildWorkflow(workflowInitData);

            TryAddWorkflow(workflowInitData.WorkflowConfiguration.Id, workflow);
        }

        public Task<WorkflowResult> StartWorkflowAsync(string workflowId) 
            => _workflows.TryGetValue(workflowId, out var workflow)
                ? workflow.StartAsync()
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

        private IWorkflow BuildWorkflow(IWorkflowInitData workflowInitData)
        {
            var workflowBuilder = _serviceProvider.GetService<IWorkflowBuilder>();

            workflowBuilder.WithConfiguration(workflowInitData.WorkflowConfiguration);

            workflowInitData.BranchesData
                .ForEach(branchData => workflowBuilder.WithBranch(branchData.Type, branchData.Configuration));
            workflowInitData.ActivitiesData
                .ForEach(activityData => workflowBuilder.WithActivity(activityData.Type, activityData.Configuration));

            return workflowBuilder.Build();
        }
    }
}
