namespace LiqWorkflow.Abstractions.Models.Configurations
{
    public class BranchTransition
    {
        public BranchTransition(
            string fromBranchId, 
            string toBranchId, 
            string fromActvityId, 
            string toActvityId)
        {
            FromBranchId = fromBranchId;
            ToBranchId = toBranchId;
            FromActvityId = fromActvityId;
            ToActvityId = toActvityId;
        }

        public string FromBranchId { get; }

        public string ToBranchId { get; }

        public string FromActvityId { get; }

        public string ToActvityId { get; }
    }
}
