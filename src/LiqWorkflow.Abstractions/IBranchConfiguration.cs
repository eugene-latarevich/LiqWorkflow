using System.Collections.Immutable;
using LiqWorkflow.Abstractions.Models.Configurations;

namespace LiqWorkflow.Abstractions
{
    public interface IBranchConfiguration
    {
        string BranchId { get; }

        int Order { get; }

        bool StartingBranch { get; }

        BranchTransition Transition { get; }

        ImmutableArray<string> ActivityIds { get; }
    }
}
