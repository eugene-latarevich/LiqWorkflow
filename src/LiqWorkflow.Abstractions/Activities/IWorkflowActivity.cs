using System.Collections.Immutable;

namespace LiqWorkflow.Abstractions.Activities
{
    public interface IWorkflowActivity : IWorkflowActivityAction
    {
        IActivityConfiguration Configuration { get; }

        ImmutableDictionary<string, IWorkflowBranch> Branches { get; }
    }
}
