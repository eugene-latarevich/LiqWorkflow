namespace LiqWorkflow.Abstractions.Models.Configurations
{
    public class ActivityTransition
    {
        public ActivityTransition(string activityFromId, string activityToId)
        {
            ActivityFromId = activityFromId;
            ActivityToId = activityToId;
        }

        public string ActivityFromId { get; }

        public string ActivityToId { get; }
    }
}
