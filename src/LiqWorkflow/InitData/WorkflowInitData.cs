using System.Collections.Generic;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;

namespace LiqWorkflow.InitData
{
    public class WorkflowInitData : IWorkflowInitData
    {
        public IWorkflowConfiguration WorkflowConfiguration { get; init; }

        public IEnumerable<IBranchInitData> BranchesData { get; init; }

        public IEnumerable<IActivityInitData> ActivitiesData { get; init; }
    }
}
