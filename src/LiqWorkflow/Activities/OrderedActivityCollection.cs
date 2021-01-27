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
        private readonly IDictionary<string, IWorkflowActivity> _activities = new Dictionary<string, IWorkflowActivity>();

        private bool _canStartFrom = true;
        private int _startFrom = 0;

        public OrderedActivityCollection(ImmutableDictionary<string, IWorkflowActivity> activities)
        {
            CreateOrderedActivities(activities);
        }

        public IWorkflowActivity this[string activityId] => _activities[activityId];

        public IOrderedActivityCollection Clone()
        {
            var activities = _activities.Select(x => x).ToImmutableDictionary();
            return new OrderedActivityCollection(activities);
        }

        public IOrderedActivityCollection StartFrom(string activityId)
        {
            var activity = _activities
                .Where(x => x.Key == activityId)
                .Select(x => x.Value)
                .FirstOrDefault();

            if (activity == null)
            {
                throw new NotFoundException("Start From activity wasn't found");
            }
            
            _startFrom = _activities.Select(x => x.Key).ToList().IndexOf(activityId);

            return this;
        }

        public IEnumerator<IWorkflowActivity> GetEnumerator()
        {
            foreach (var keyActivityPair in _activities.Skip(_canStartFrom ? _startFrom - 1 : 0))
            {
                _canStartFrom = false;

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
