namespace LiqWorkflow.Abstractions.Models.Configurations
{
    public class ActivityConfiguration
    {
        public ActivityConfiguration(
            string activityId, 
            ActivityTransition transition,
            bool isBranchStartPoint = false,
            bool isBranchFinishPoint = false)
        {
            ActivityId = activityId;
            IsBranchStartPoint = isBranchStartPoint;
            IsBranchFinishPoint = isBranchFinishPoint;
            Transition = transition;
        }

        public string ActivityId { get; }

        public bool IsBranchStartPoint { get; }

        public bool IsBranchFinishPoint { get; }

        public ActivityTransition Transition { get; }
    }
}
