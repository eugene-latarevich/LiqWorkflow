using System.Threading;
using LiqWorkflow.Abstractions.Containers;
using LiqWorkflow.Abstractions.Models.Settings;

namespace LiqWorkflow.Abstractions
{
    public interface IWorkflowConfiguration
    {
        string Id { get; init; }

        string Name { get; init; }

        RetrySetting RetrySetting { get; init; }

        CancellationTokenSource CancellationTokenSource { get; init; }

        IContainer Services { get; init; }
    }
}
