using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions.Models;

namespace LiqWorkflow.Abstractions.Activities
{
    public interface IRestorableWorkflowActivitity : IWorkflowExecutableActivity
    {
        Task<WorkflowResult<ActivityData>> RestoreAsync(CancellationToken cancellationToken = default);
    }
}
