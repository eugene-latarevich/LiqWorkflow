using System.Collections.Generic;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;

namespace LiqWorkflow.Abstractions
{
    public interface IWorkflowInitData
    {
        IWorkflowConfiguration WorkflowConfiguration { get; }

        IEnumerable<IBranchInitData> BranchesData { get; }

        IEnumerable<IActivityInitData> ActivitiesData { get; }
    }
}
