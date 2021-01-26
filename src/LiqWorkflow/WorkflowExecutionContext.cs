using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Models;

namespace LiqWorkflow
{
    public abstract class WorkflowExecutionContext
    {
        private readonly ConcurrentDictionary<string, IWorkflow> _workflows = new ConcurrentDictionary<string, IWorkflow>();

        protected WorkflowExecutionContext()
        {

        }

        public bool TryAddWorkflow(string workflowId, IWorkflow workflow) => _workflows.TryAdd(workflowId, workflow);

        public bool TryRemoveWorkflow(string workflowId) => _workflows.TryRemove(workflowId, out _);

        protected abstract Task OnActivityResultAsync(ActivityData result, CancellationToken cancellation);

        protected abstract Task OnLogAsync(OnLogData logData, CancellationToken cancellationToken);
    }
}
