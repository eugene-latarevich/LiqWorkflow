using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions;

namespace LiqWorkflow
{
    public class Workflow : IWorkflow
    {
        private CancellationTokenSource _cts;

        public Workflow(IWorkflowConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IWorkflowConfiguration Configuration { get; }

        public Task StartAsync()
        {
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            return Task.CompletedTask;
        }
    }
}
