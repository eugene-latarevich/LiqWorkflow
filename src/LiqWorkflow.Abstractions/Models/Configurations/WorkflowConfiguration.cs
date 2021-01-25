using System.Threading;

namespace LiqWorkflow.Abstractions.Models.Configurations
{
    public class WorkflowConfiguration : IWorkflowConfiguration
    {
        public WorkflowConfiguration(
            string id, 
            string name, 
            SchedulerConfiguration schedulerConfiguration)
        {
            Id = id;
            Name = name;
            SchedulerConfiguration = schedulerConfiguration;

            CancellationTokenSource = new CancellationTokenSource();
        }

        public string Id { get; }

        public string Name { get; }

        public SchedulerConfiguration SchedulerConfiguration { get; }

        public CancellationTokenSource CancellationTokenSource { get; }
    }
}
