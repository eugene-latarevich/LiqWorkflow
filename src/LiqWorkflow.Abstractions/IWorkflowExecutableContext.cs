using System.Threading.Tasks;
using LiqWorkflow.Abstractions.Models;

namespace LiqWorkflow.Abstractions
{
    public interface IWorkflowExecutableContext
    {
        Task<WorkflowResult> StartWorkflowAsync(string workflowId);

        Task<WorkflowResult> StopWorkflowAsync(string workflowId);
    }
}
