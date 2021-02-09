namespace LiqWorkflow.Abstractions.Models.Configurations
{
    public class BranchTransition
    {
        public string FromBranchId { get; init; }

        public string ToBranchId { get; init; }

        public string FromActvityId { get; init; }

        public string ToActvityId { get; init; }
    }
}
