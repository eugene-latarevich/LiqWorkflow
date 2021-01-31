using System;
using LiqWorkflow.Abstractions.Activities;

namespace LiqWorkflow.Abstractions.Models.Configurations
{
    public class ActivityConfiguration : IActivityConfiguration
    {
        public ActivityConfiguration(
            string activityId, 
            ActivityTransition transition,
            IServiceProvider serviceProvider,
            bool isBranchStartPoint = false,
            bool isBranchFinishPoint = false)
        {
            ActivityId = activityId;
            IsBranchStartPoint = isBranchStartPoint;
            IsBranchFinishPoint = isBranchFinishPoint;
            Transition = transition;
            ServiceProvider = serviceProvider;
        }

        public string ActivityId { get; }

        public bool IsBranchStartPoint { get; }

        public bool IsBranchFinishPoint { get; }

        public ActivityTransition Transition { get; }

        public IServiceProvider ServiceProvider { get; }
    }
}
