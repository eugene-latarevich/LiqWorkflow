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
            bool isBranchFinishPoint = false,
            bool restorePoint = false)
        {
            ActivityId = activityId;
            IsBranchStartPoint = isBranchStartPoint;
            IsBranchFinishPoint = isBranchFinishPoint;
            RestorePoint = restorePoint;
            Transition = transition;
            ServiceProvider = serviceProvider;
        }

        public string ActivityId { get; }

        public bool IsBranchStartPoint { get; }

        public bool IsBranchFinishPoint { get; }

        public bool RestorePoint { get; }

        public ActivityTransition Transition { get; }

        public IServiceProvider ServiceProvider { get; }
    }
}
