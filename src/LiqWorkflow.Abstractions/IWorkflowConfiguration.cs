using System.Threading;
using LiqWorkflow.Abstractions.Models.Settings;

namespace LiqWorkflow.Abstractions
{
    public interface IWorkflowConfiguration
    {
        string Id { get; }

        string Name { get; }

        RetrySetting RetrySetting { get; }

        CancellationTokenSource CancellationTokenSource { get; }
    }
}
