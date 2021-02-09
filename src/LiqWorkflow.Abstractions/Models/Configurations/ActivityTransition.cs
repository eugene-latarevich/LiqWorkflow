namespace LiqWorkflow.Abstractions.Models.Configurations
{
    public class ActivityTransition
    {
        public string ActivityFromId { get; init; }

        public string ActivityToId { get; init; }

        public bool HasChildBranches { get; init; }
    }
}
