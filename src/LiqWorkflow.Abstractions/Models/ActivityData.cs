using System.Collections.Generic;

namespace LiqWorkflow.Abstractions.Models
{
    public class ActivityData
    {
        public ActivityData(string activityId, string activityToId, Dictionary<string, ActivityDataValue> values)
        {
            ActivityId = activityId;
            ActivityToId = activityToId;
            Values = values;
        }

        public string ActivityId { get; }

        public string ActivityToId { get; }

        public IDictionary<string, ActivityDataValue> Values { get; }
    }
}
