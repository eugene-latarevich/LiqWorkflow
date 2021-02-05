using System;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;

namespace LiqWorkflow.Abstractions
{
    public interface IWorkflowBuilder
    {
        IWorkflowBuilder WithConfiguration(IWorkflowConfiguration configuration);

        IWorkflowBuilder WithBranch<TBranch>(IBranchConfiguration configuration)
            where TBranch : class, IWorkflowBranch;

        IWorkflowBuilder WithBranch(Type type, IBranchConfiguration configuration);

        IWorkflowBuilder WithActivity<TActivity>(IActivityConfiguration configuration)
            where TActivity : class, IWorkflowActivity;

        IWorkflowBuilder WithActivity(Type type, IActivityConfiguration configuration);

        IWorkflow Build();
    }
}
