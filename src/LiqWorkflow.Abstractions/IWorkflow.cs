using System.Threading.Tasks;
using LiqWorkflow.Abstractions.Models;
using LiqWorkflow.Abstractions.Models.Enums;

namespace LiqWorkflow.Abstractions
{
    public interface IWorkflow
    {
        WorkflowStatus Status { get; }

        IWorkflowConfiguration Configuration { get; }

        Task<WorkflowResult> StartAsync();

        Task<WorkflowResult> StopAsync();
    }
}
