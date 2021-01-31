using System.Collections.Immutable;
using LiqWorkflow.Abstractions.Branches;

namespace LiqWorkflow.Abstractions.Activities
{
    public interface IWorkflowActivity : IWorkflowActivityAction
    {
        IActivityConfiguration Configuration { get; }

        ImmutableDictionary<string, IWorkflowBranch> Branches { get; }
    }
}
