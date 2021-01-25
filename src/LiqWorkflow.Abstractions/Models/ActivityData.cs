using System.Collections.Generic;

namespace LiqWorkflow.Abstractions.Models
{
    public class ActivityData
    {
        public ActivityData(Dictionary<string, ActivityDataValue> values)
        {
            Values = values;
        }

        public IDictionary<string, ActivityDataValue> Values { get; }
    }
}
