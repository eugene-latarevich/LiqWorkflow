using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions.Models;

namespace LiqWorkflow.Abstractions.Activities
{
    public interface IWorkflowActivity
    {
        Task ExecuteAsync(ActivityData data, CancellationToken cancellationToken = default);
    }
}
