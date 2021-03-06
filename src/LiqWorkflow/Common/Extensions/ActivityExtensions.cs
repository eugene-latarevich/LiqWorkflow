﻿using System.Collections.Generic;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Containers;
using LiqWorkflow.Abstractions.Models;
using LiqWorkflow.Abstractions.Models.Builder;

namespace LiqWorkflow.Common.Extensions
{
    static class ActivityExtensions
    {
        public static bool ValidateBranchStartEnd(this IEnumerable<IWorkflowActivity> activities)
        {
            int startPointCount = 0;
            int finishPointCount = 0;
            foreach (var activity in activities)
            {
                if (activity.Configuration.IsBranchStartPoint)
                {
                    startPointCount++;
                }

                if (activity.Configuration.IsBranchFinishPoint)
                {
                    finishPointCount++;
                }
            }

            return startPointCount == 1 && finishPointCount == 1;
        }

        public static bool ValidateInnerBranches(this IEnumerable<IWorkflowActivity> activities)
        {
            foreach (var activity in activities)
            {
                foreach (var activityBranch in activity.Branches)
                {
                    var validBranch = activityBranch.Value.IsValid();
                    if (!validBranch)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static string GetActivityToId(this KeyValuePair<string, IWorkflowActivity> keyActivityPair) => keyActivityPair.Value.Configuration.Transition.ActivityToId;

        public static CreatingActivityConfiguration CreateActivityConfigurationForBuilder(this IActivityInitData initData, IContainer container) 
            => new(container)
            {
                ActiviyKey = initData.ActivityKey,
                ActivityActionKey = initData.ActivityActionKey,
                RestoredActivityKey = initData.RestoredActivityKey,
                Configuration = initData.Configuration,
                BranchIds = initData.BranchIds,
            };

        public static ActivityData Create(this IActivityConfiguration configuration) 
            => new()
            {
                ActivityId = configuration.ActivityId,
                ActivityToId = configuration.ActivityToId,
                RestorePoint = configuration.RestorePoint,
                FromConnectedBranch = false,
            };
    }
}
