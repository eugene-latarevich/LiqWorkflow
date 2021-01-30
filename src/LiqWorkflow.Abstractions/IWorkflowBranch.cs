using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions.Activities;

namespace LiqWorkflow.Abstractions
{
    public interface IWorkflowBranch
    {
        IBranchConfiguration Configuration { get; }

        IOrderedActivityCollection Activities { get; }

        Task PulseAsync(CancellationToken cancellationToken);

        bool IsValid();
    }
}
