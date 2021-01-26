using System.Collections.Generic;
using LiqWorkflow.Abstractions.Activities;

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
    }
}
