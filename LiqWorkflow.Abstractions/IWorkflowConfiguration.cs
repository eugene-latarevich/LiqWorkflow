using System.Collections.Immutable;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Models;

namespace LiqWorkflow.Abstractions
{
    public interface IWorkflowConfiguration
    {
        string Id { get; }

        string Name { get; }

        SchedulerConfiguration SchedulerConfiguration { get; }

        ImmutableDictionary<int, IWorkflowActivity> Activities { get; }
    }
}
