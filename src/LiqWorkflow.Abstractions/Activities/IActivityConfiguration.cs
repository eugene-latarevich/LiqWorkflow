using LiqWorkflow.Abstractions.Containers;
using LiqWorkflow.Abstractions.Models.Configurations;

namespace LiqWorkflow.Abstractions.Activities
{
    public interface IActivityConfiguration
    {
        string ActivityId { get; init; }

        string ActivityToId { get; init; }

        bool IsBranchStartPoint { get; init; }

        bool IsBranchFinishPoint { get; init; }

        bool RestorePoint { get; init; }

        ActivityTransition Transition { get; init; }

        IContainer Services { get; init; }
    }
}
