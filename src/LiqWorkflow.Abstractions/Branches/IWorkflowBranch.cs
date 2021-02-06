using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions.Activities;

namespace LiqWorkflow.Abstractions.Branches
{
    public interface IWorkflowBranch
    {
        IBranchConfiguration Configuration { get; }

        IOrderedActivityCollection Activities { get; }

        Task PulseAsync(CancellationToken cancellationToken);

        Task RestoreAsync(CancellationToken cancellationToken);

        bool IsValid();
    }
}
