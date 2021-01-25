using System.Collections.Immutable;
using LiqWorkflow.Abstractions.Activities;

namespace LiqWorkflow.Abstractions
{
    public interface IWorkflowBranch
    {
        ImmutableDictionary<string, IWorkflowActivity> Activities { get; set; }

        bool IsValid();
    }
}
