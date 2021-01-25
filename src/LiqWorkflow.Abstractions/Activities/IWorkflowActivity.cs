using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions.Models;

namespace LiqWorkflow.Abstractions.Activities
{
    public interface IWorkflowActivity
    {
        ImmutableDictionary<string, IWorkflowBranch> Branches { get; set; }

        Task ExecuteAsync(ActivityData data, CancellationToken cancellationToken = default);
    }
}
