using System.Collections.Immutable;
using LiqWorkflow.Abstractions.Branches;

namespace LiqWorkflow.Abstractions.Activities
{
    public interface IWorkflowActivity
    {
        IActivityConfiguration Configuration { get; }

        ImmutableDictionary<string, IWorkflowBranch> Branches { get; }
    }
}
