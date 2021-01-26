using System.Threading;

namespace LiqWorkflow.Abstractions
{
    public interface IWorkflowConfiguration
    {
        string Id { get; }

        string Name { get; }

        CancellationTokenSource CancellationTokenSource { get; }
    }
}
