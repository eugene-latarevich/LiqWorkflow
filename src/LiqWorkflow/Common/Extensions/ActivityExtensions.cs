using System;
using System.Collections.Generic;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Models.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LiqWorkflow.Common.Extensions
{
    static class ActivityExtensions
    {
        public static IRestorableWorkflowActivitity AsRestorable(this IWorkflowExecutableActivity activity, IServiceProvider serviceProvider)
        {
            //TODO get object fom own container
            return serviceProvider.GetService<IRestorableWorkflowActivitity>();
        }

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

        public static CreatingActivityConfiguration CreateActivityConfigurationForBuilder(this IActivityInitData initData) => new CreatingActivityConfiguration
        {
            ActiviyType = initData.ActivityType,
            ExecutableActivityType = initData.ExecutableActivityType,
            Configuration = initData.Configuration,
            BranchIds = initData.BranchIds,
            Parameters = initData.Parameters,
        };
    }
}
