using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;

namespace LiqWorkflow.Abstractions
{
    public interface IWorkflowBuilder
    {
        IWorkflowBuilder WithConfiguration(IWorkflowConfiguration configuration);

        IWorkflowBuilder WithBranch(IBranchInitData initData);

        IWorkflowBuilder WithActivity(IActivityInitData initData);

        IWorkflow Build();
    }
}
