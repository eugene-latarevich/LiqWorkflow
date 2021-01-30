using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions.Models;

namespace LiqWorkflow.Abstractions.Activities
{
    public interface IWorkflowActivityAction
    {
        Task<WorkflowResult<ActivityData>> ExecuteAsync(ActivityData data, CancellationToken cancellationToken = default);
    }
}
