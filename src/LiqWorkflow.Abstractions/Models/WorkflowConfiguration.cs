using System.Collections.Immutable;
using System.Threading;
using LiqWorkflow.Abstractions.Activities;

namespace LiqWorkflow.Abstractions.Models
{
    public class WorkflowConfiguration : IWorkflowConfiguration
    {
        public WorkflowConfiguration(
            string id, 
            string name, 
            SchedulerConfiguration schedulerConfiguration,
            ImmutableDictionary<string, IWorkflowActivity> activities)
        {
            Id = id;
            Name = name;
            SchedulerConfiguration = schedulerConfiguration;
            Activities = activities;

            CancellationTokenSource = new CancellationTokenSource();
        }

        public string Id { get; }

        public string Name { get; }

        public SchedulerConfiguration SchedulerConfiguration { get; }

        public ImmutableDictionary<string, IWorkflowActivity> Activities { get; }

        public CancellationTokenSource CancellationTokenSource { get; }
    }
}
