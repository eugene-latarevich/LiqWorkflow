namespace LiqWorkflow.Abstractions.Models.Configurations
{
    public class ActivityConfiguration
    {
        public ActivityConfiguration(
            string activityId, 
            string activityToId,
            bool isBranchStartPoint = false,
            bool isBranchFinishPoint = false)
        {
            ActivityId = activityId;
            ActivityToId = activityToId;
            IsBranchStartPoint = isBranchStartPoint;
            IsBranchFinishPoint = isBranchFinishPoint;
        }

        public string ActivityId { get; }

        public string ActivityToId { get; }

        public bool IsBranchStartPoint { get; }

        public bool IsBranchFinishPoint { get; }
    }
}
