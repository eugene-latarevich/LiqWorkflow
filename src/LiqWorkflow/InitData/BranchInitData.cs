using LiqWorkflow.Abstractions.Branches;

namespace LiqWorkflow.InitData
{
    public class BranchInitData : IBranchInitData
    {
        public BranchInitData(IBranchConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IBranchConfiguration Configuration { get; }
    }
}
