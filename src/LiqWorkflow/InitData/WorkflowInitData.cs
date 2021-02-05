using System.Collections.Generic;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;

namespace LiqWorkflow.InitData
{
    public class WorkflowInitData : IWorkflowInitData
    {
        public WorkflowInitData(
            IWorkflowConfiguration workflowConfiguration,
            IEnumerable<BranchInitData> branchesData,
            IEnumerable<IActivityInitData> activitiesData)
        {
            WorkflowConfiguration = workflowConfiguration;
            BranchesData = branchesData;
            ActivitiesData = activitiesData;
        }

        public IWorkflowConfiguration WorkflowConfiguration { get; }

        public IEnumerable<IBranchInitData> BranchesData { get; }

        public IEnumerable<IActivityInitData> ActivitiesData { get; }
    }
}
