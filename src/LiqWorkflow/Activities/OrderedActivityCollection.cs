using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Exceptions;

namespace LiqWorkflow.Activities
{
    public class OrderedActivityCollection : IOrderedActivityCollection
    {
        public IDictionary<string, IWorkflowActivity> _activities = new Dictionary<string, IWorkflowActivity>();

        public OrderedActivityCollection(ImmutableDictionary<string, IWorkflowActivity> activities)
        {
            CreateOrderedActivities(activities);
        }

        public IWorkflowActivity this[string activityId] => _activities[activityId];

        public IEnumerator<IWorkflowActivity> GetEnumerator()
        {
            foreach (var keyActivityPair in _activities)
            {
                yield return keyActivityPair.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void CreateOrderedActivities(ImmutableDictionary<string, IWorkflowActivity> activities)
        {
            for (int x = 0; x < activities.Count; x++)
            {
                KeyValuePair<string, IWorkflowActivity> keyActivityPair = default;

                if (x == 0)
                {
                    keyActivityPair = activities.FirstOrDefault(z => z.Value.Configuration.IsBranchStartPoint);
                    if (keyActivityPair.Value == null)
                    {
                        throw new NotFoundException("Start activity wasn't found");
                    }
                }
                else
                {
                    var toActivityId = keyActivityPair.Value.Configuration.Transition.ActivityToId;
                    keyActivityPair = activities.FirstOrDefault(z => z.Value.Configuration.ActivityId == toActivityId);
                    if (keyActivityPair.Value == null)
                    {
                        throw new NotFoundException("Continuation activity wasn't found");
                    }
                }

                _activities.Add(keyActivityPair);
            }
        }
    }
}
