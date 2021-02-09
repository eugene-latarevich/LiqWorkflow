using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Containers;

namespace LiqWorkflow.Abstractions.Models.Configurations
{
    public class ActivityConfiguration : IActivityConfiguration
    {
        public string ActivityId { get; init; }

        public string ActivityToId { get; init; }

        public bool IsBranchStartPoint { get; init; }

        public bool IsBranchFinishPoint { get; init; }

        public bool RestorePoint { get; init; }

        public ActivityTransition Transition { get; init; }

        public IContainer Services { get; init; }
    }
}
