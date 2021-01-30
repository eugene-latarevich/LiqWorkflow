namespace LiqWorkflow.Abstractions.Models.Configurations
{
    public class ActivityTransition
    {
        public ActivityTransition(string activityFromId, string activityToId, bool hasChildBranches)
        {
            ActivityFromId = activityFromId;
            ActivityToId = activityToId;
            HasChildBranches = hasChildBranches;
        }

        public string ActivityFromId { get; }

        public string ActivityToId { get; }

        public bool HasChildBranches { get; }
    }
}
