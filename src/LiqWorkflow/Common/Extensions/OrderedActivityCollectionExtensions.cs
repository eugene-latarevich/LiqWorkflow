using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Activities;

namespace LiqWorkflow.Common.Extensions
{
    static class OrderedActivityCollectionExtensions
    {
        public static IOrderedActivityCollection Clone(this IOrderedActivityCollection collection)
        {
            var activities = collection
                .Select(activity => new KeyValuePair<string, IWorkflowActivity>(activity.Configuration.ActivityId, activity))
                .ToImmutableDictionary();
            return new OrderedActivityCollection(activities);
        }

        public static IEnumerable<IWorkflowExecutableActivity> StartFrom(this IOrderedActivityCollection collection, string activityId)
        {
            var skiped = false; 
            foreach (var activity in collection)
            {
                if (!string.IsNullOrEmpty(activityId))
                {
                    if (!skiped && activity.Configuration.ActivityId != activityId)
                    {
                        continue;
                    }

                    skiped = true;
                }

                yield return activity;
            }
        }
    }
}
