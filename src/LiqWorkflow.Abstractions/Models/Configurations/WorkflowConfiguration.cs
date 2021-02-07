using System.Threading;
using LiqWorkflow.Abstractions.Containers;
using LiqWorkflow.Abstractions.Models.Settings;

namespace LiqWorkflow.Abstractions.Models.Configurations
{
    public class WorkflowConfiguration : IWorkflowConfiguration
    {
        public WorkflowConfiguration(
            string id, 
            string name, 
            RetrySetting retrySetting, 
            IContainer services)
        {
            Id = id;
            Name = name;
            RetrySetting = retrySetting;
            Services = services;
            CancellationTokenSource = new CancellationTokenSource();
        }

        public string Id { get; }

        public string Name { get; }

        public RetrySetting RetrySetting { get; }

        public IContainer Services { get; }

        public CancellationTokenSource CancellationTokenSource { get; }
    }
}
