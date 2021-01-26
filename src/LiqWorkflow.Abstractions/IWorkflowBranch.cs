using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions.Activities;

namespace LiqWorkflow.Abstractions
{
    public interface IWorkflowBranch
    {
        ImmutableDictionary<string, IWorkflowActivity> Activities { get; }

        Task PulseAsync(CancellationToken cancellationToken);

        bool IsValid();
    }
}
