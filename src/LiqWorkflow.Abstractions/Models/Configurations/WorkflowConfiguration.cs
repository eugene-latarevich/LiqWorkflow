using System.Threading;

namespace LiqWorkflow.Abstractions.Models.Configurations
{
    public class WorkflowConfiguration : IWorkflowConfiguration
    {
        public WorkflowConfiguration(string id, string name)
        {
            Id = id;
            Name = name;
            CancellationTokenSource = new CancellationTokenSource();
        }

        public string Id { get; }

        public string Name { get; }

        public CancellationTokenSource CancellationTokenSource { get; }
    }
}
