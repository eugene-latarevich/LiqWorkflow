using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Exceptions;

namespace LiqWorkflow
{
    public class Workflow : IWorkflow
    {
        private IEnumerable<IWorkflowBranch> _branches;

        public Workflow(
            IWorkflowConfiguration configuration,
            IEnumerable<IWorkflowBranch> branches)
        {
            Configuration = configuration;

            _branches = branches;
        }

        public IWorkflowConfiguration Configuration { get; }

        public async Task StartAsync()
        {
            ThrowIfNotValidConfiguration();

        }

        public async Task StopAsync()
        {
            ThrowIfNotValidConfiguration();

        }

        private void ThrowIfNotValidConfiguration()
        {
            if (_branches == null || !_branches.Any() || _branches.Any(x => !x.IsValid()))
            {
                throw new InvalidWorkflowConfigurationException();
            }
        }
    }
}
