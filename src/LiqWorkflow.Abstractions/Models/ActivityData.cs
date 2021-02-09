using System.Collections.Generic;

namespace LiqWorkflow.Abstractions.Models
{
    public class ActivityData
    {
        public ActivityData(
            string activityId, 
            string activityToId, 
            bool restorePoint,
            bool fromConnectedBranch)
        {
            ActivityId = activityId;
            ActivityToId = activityToId;
            RestorePoint = restorePoint;
            FromConnectedBranch = fromConnectedBranch;
            Values = new Dictionary<string, ActivityDataValue>();
        }

        public ActivityData(
            string activityId, 
            string activityToId, 
            bool restorePoint,
            bool fromConnectedBranch, 
            Dictionary<string, ActivityDataValue> values)
            : this(activityId, activityToId, restorePoint, fromConnectedBranch)
        {
            Values = values;
        }

        public string ActivityId { get; }

        public string ActivityToId { get; }

        public bool FromConnectedBranch { get; }

        public bool RestorePoint { get; }

        public IDictionary<string, ActivityDataValue> Values { get; }

        public ActivityData Map(ActivityData data)
        {
            foreach (var value in Values)
            {
                if (!data.Values.ContainsKey(value.Key))
                {
                    data.Values.Add(value);
                }
            }

            return this;
        }

        public string GetStartFromActivityId() => RestorePoint || FromConnectedBranch ? ActivityToId : string.Empty;
    }
}
