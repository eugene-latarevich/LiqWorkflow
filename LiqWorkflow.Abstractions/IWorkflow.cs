using System.Threading;
using System.Threading.Tasks;

namespace LiqWorkflow.Abstractions
{
    public interface IWorkflow
    {
        IWorkflowConfiguration Configuration { get; set; }

        Task StartAsync(CancellationToken cancellationToken = default);

        Task StopAsync(CancellationToken cancellationToken = default);
    }
}
