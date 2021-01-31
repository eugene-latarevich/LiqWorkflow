using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions.Models;

namespace LiqWorkflow.Abstractions.Branches
{
    public interface IWorkflowBranchContinuation
    {
        Task ContinueWithAsync(ActivityData result, CancellationToken cancellationToken);
    }
}
