using LiqWorkflow.Abstractions.Activities;

namespace LiqWorkflow.Abstractions.Builders
{
    public interface IWorkflowBuilder
    {
        IWorkflowBuilder WithConfiguration(IWorkflowConfiguration configuration);

        IWorkflowBuilder WithBranch<TBranch>(IBranchConfiguration configuration)
            where TBranch : class, IWorkflowBranch;

        IWorkflowBuilder WithActivity<TActivity>(IActivityConfiguration configuration)
            where TActivity : class, IWorkflowActivity;

        IWorkflow Build();
    }
}
