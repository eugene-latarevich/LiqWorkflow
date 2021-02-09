using LiqWorkflow.Abstractions;

namespace LiqWorkflow.Common.Extensions
{
    static class WorkflowBuilderExtensions
    {
        public static IWorkflow BuildWorkflow(this IWorkflowBuilder workflowBuilder, IWorkflowInitData workflowInitData)
        {
            workflowBuilder.WithConfiguration(workflowInitData.WorkflowConfiguration);

            workflowInitData.BranchesData.ForEach(branchData => workflowBuilder.WithBranch(branchData));
            workflowInitData.ActivitiesData.ForEach(activityData => workflowBuilder.WithActivity(activityData));

            return workflowBuilder.Build();
        }
    }
}
