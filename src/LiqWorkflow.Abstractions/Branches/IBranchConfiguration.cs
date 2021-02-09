using System.Collections.Immutable;
using LiqWorkflow.Abstractions.Models.Configurations;

namespace LiqWorkflow.Abstractions.Branches
{
    public interface IBranchConfiguration
    {
        string BranchId { get; init; }

        int Order { get; init; }

        bool StartingBranch { get; init; }

        BranchTransition Transition { get; init; }

        ImmutableArray<string> ActivityIds { get; init; }
    }
}
