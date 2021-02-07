﻿using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Containers;

namespace LiqWorkflow.Abstractions.Models.Configurations
{
    public class ActivityConfiguration : IActivityConfiguration
    {
        public ActivityConfiguration(
            string activityId, 
            ActivityTransition transition,
            IContainer services,
            bool isBranchStartPoint = false,
            bool isBranchFinishPoint = false,
            bool restorePoint = false)
        {
            ActivityId = activityId;
            IsBranchStartPoint = isBranchStartPoint;
            IsBranchFinishPoint = isBranchFinishPoint;
            RestorePoint = restorePoint;
            Transition = transition;
            Services = services;
        }

        public string ActivityId { get; }

        public bool IsBranchStartPoint { get; }

        public bool IsBranchFinishPoint { get; }

        public bool RestorePoint { get; }

        public ActivityTransition Transition { get; }

        public IContainer Services { get; }
    }
}
