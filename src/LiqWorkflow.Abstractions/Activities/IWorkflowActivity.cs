using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions.Models;
using LiqWorkflow.Abstractions.Models.Configurations;

namespace LiqWorkflow.Abstractions.Activities
{
    public interface IWorkflowActivity
    {
        ActivityConfiguration Configuration { get; }

        ImmutableDictionary<string, IWorkflowBranch> Branches { get; }

        Task ExecuteAsync(ActivityData data, CancellationToken cancellationToken = default);
    }
}
