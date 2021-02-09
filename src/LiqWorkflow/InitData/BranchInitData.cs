using LiqWorkflow.Abstractions.Branches;

namespace LiqWorkflow.InitData
{
    public class BranchInitData : IBranchInitData
    {
        public IBranchConfiguration Configuration { get; init; }
    }
}
