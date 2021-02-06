using LiqWorkflow.Abstractions.Branches;
using LiqWorkflow.Abstractions.Models.Builder;

namespace LiqWorkflow.Common.Extensions
{
    static class BranchExtensions
    {
        public static CreatingBranchConfiguration CreateBranchConfigurationForBuilder(this IBranchInitData initData) => new CreatingBranchConfiguration
        {
            Type = initData.Type, 
            Configuration = initData.Configuration,
        };
    }
}
