using System.Collections.Immutable;
using System.Threading;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Models;

namespace LiqWorkflow.Abstractions
{
    public interface IWorkflowConfiguration
    {
        string Id { get; }

        string Name { get; }

        CancellationTokenSource CancellationTokenSource { get; }

        SchedulerConfiguration SchedulerConfiguration { get; }

        ImmutableDictionary<string, IWorkflowActivity> Activities { get; }
    }
}
