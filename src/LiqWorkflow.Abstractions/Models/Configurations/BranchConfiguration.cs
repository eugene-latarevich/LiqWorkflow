using System.Collections.Immutable;
using LiqWorkflow.Abstractions.Branches;

namespace LiqWorkflow.Abstractions.Models.Configurations
{
    public class BranchConfiguration : IBranchConfiguration
    {
        public string BranchId { get; init; }

        public int Order { get; init; }

        public bool StartingBranch { get; init; }

        public BranchTransition Transition { get; init; }

        public ImmutableArray<string> ActivityIds { get; init; }
    }
}
