using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Common.Extensions;
using LiqWorkflow.Exceptions;

namespace LiqWorkflow.Activities
{
    public class OrderedActivityCollection : IOrderedActivityCollection
    {
        private readonly IDictionary<string, IWorkflowActivity> _activities = new Dictionary<string, IWorkflowActivity>();

        public OrderedActivityCollection(IDictionary<string, IWorkflowActivity> activities)
        {
            ThrowIfAnyActivityNotExecutable(activities);
            CreateOrderedActivities(activities);
        }

        public IWorkflowActivity this[string activityId] => _activities[activityId];

        public IEnumerator<IWorkflowExecutableActivity> GetEnumerator()
        {
            foreach (var keyActivityPair in _activities)
            {
                if (keyActivityPair.Value is IWorkflowExecutableActivity executableActivity)
                {
                    yield return executableActivity;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void ThrowIfAnyActivityNotExecutable(IDictionary<string, IWorkflowActivity> activities)
        {
            activities
                .Select(x =>x.Value)
                .ForEach(activity =>
                {
                    if (!(activity is IWorkflowExecutableActivity))
                    {
                        throw new UnknownTypeException($"Activity must be executable {typeof(IWorkflowExecutableActivity)}. ActivityId={activity.Configuration.ActivityId}");
                    }
                });
        }

        private void CreateOrderedActivities(IDictionary<string, IWorkflowActivity> activities)
        {
            KeyValuePair<string, IWorkflowActivity> keyActivityPair = default;

            for (int x = 0; x < activities.Count; x++)
            {
                keyActivityPair = x == 0 
                    ? FindActivity(activities, z => z.Value.Configuration.IsBranchStartPoint) 
                    : FindActivity(activities, z => z.Value.Configuration.ActivityId == keyActivityPair.GetActivityToId());

                _activities.Add(keyActivityPair);
            }
        }

        private KeyValuePair<string, IWorkflowActivity> FindActivity(IDictionary<string, IWorkflowActivity> activities, Func<KeyValuePair<string, IWorkflowActivity>, bool> predicate)
        {
            var keyActivityPair = activities.FirstOrDefault(predicate);
            return keyActivityPair.Value == null ? throw new NotFoundException("Activity wasn't found") : keyActivityPair;
        }
    }
}
