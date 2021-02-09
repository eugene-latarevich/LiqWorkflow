using System.Threading;
using LiqWorkflow.Abstractions.Containers;
using LiqWorkflow.Abstractions.Models.Settings;

namespace LiqWorkflow.Abstractions.Models.Configurations
{
    public class WorkflowConfiguration : IWorkflowConfiguration
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public RetrySetting RetrySetting { get; init; }

        public IContainer Services { get; init; }

        public CancellationTokenSource CancellationTokenSource { get; init; }
    }
}
