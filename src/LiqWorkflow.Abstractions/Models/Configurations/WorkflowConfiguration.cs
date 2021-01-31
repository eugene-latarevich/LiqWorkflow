using System;
using System.Threading;
using LiqWorkflow.Abstractions.Models.Settings;

namespace LiqWorkflow.Abstractions.Models.Configurations
{
    public class WorkflowConfiguration : IWorkflowConfiguration
    {
        public WorkflowConfiguration(
            string id, 
            string name, 
            RetrySetting retrySetting, 
            IServiceProvider serviceProvider)
        {
            Id = id;
            Name = name;
            RetrySetting = retrySetting;
            ServiceProvider = serviceProvider;
            CancellationTokenSource = new CancellationTokenSource();
        }

        public string Id { get; }

        public string Name { get; }

        public RetrySetting RetrySetting { get; }

        public IServiceProvider ServiceProvider { get; }

        public CancellationTokenSource CancellationTokenSource { get; }
    }
}
