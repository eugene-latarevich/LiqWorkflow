using System.Collections.Generic;

namespace LiqWorkflow.Abstractions.Models
{
    public class ActivityData
    {
        public ActivityData(string activityId, Dictionary<string, ActivityDataValue> values)
        {
            ActivityId = activityId;
            Values = values;
        }

        public string ActivityId { get; }

        public IDictionary<string, ActivityDataValue> Values { get; }
    }
}
