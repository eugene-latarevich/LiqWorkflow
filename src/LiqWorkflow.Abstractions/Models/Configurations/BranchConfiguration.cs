using System.Collections.Immutable;
using LiqWorkflow.Abstractions.Branches;

namespace LiqWorkflow.Abstractions.Models.Configurations
{
    public class BranchConfiguration : IBranchConfiguration
    {
        public BranchConfiguration(
            string branchId, 
            int order, 
            bool startingBranch, 
            BranchTransition transition, 
            ImmutableArray<string> activityIds)
        {
            BranchId = branchId;
            Order = order;
            StartingBranch = startingBranch;
            Transition = transition;
            ActivityIds = activityIds;
        }

        public string BranchId { get; }

        public int Order { get; }

        public bool StartingBranch { get; }

        public BranchTransition Transition { get; }

        public ImmutableArray<string> ActivityIds { get; }
    }
}
