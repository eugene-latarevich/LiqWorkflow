using System.Collections.Generic;

namespace LiqWorkflow.Abstractions.Models
{
    public class ActivityData
    {
        public string ActivityId { get; init; }

        public string ActivityToId { get; init; }

        public bool FromConnectedBranch { get; init; }

        public bool RestorePoint { get; init; }

        public IDictionary<string, ActivityDataValue> Values { get; init; } = new Dictionary<string, ActivityDataValue>();

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
