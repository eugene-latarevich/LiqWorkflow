using System.Threading.Tasks;

namespace LiqWorkflow.Abstractions
{
    public interface IWorkflow
    {
        IWorkflowConfiguration Configuration { get; }

        Task StartAsync();

        Task StopAsync();
    }
}
