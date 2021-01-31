using System;
using LiqWorkflow.Abstractions.Models.Configurations;

namespace LiqWorkflow.Abstractions.Activities
{
    public interface IActivityConfiguration
    {
        string ActivityId { get; }

        bool IsBranchStartPoint { get; }

        bool IsBranchFinishPoint { get; }

        ActivityTransition Transition { get; }

        IServiceProvider ServiceProvider { get; }
    }
}
