using LiqWorkflow.Abstractions.Branches;
using LiqWorkflow.Abstractions.Containers;
using LiqWorkflow.Abstractions.Models.Builder;

namespace LiqWorkflow.Common.Extensions
{
    static class BranchExtensions
    {
        public static CreatingBranchConfiguration CreateBranchConfigurationForBuilder(this IBranchInitData initData, IContainer container)
            => new(container) {Configuration = initData.Configuration};
    }
}
