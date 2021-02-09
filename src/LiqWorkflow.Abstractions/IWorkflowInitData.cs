using System.Collections.Generic;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;

namespace LiqWorkflow.Abstractions
{
    public interface IWorkflowInitData
    {
        IWorkflowConfiguration WorkflowConfiguration { get; init; }

        IEnumerable<IBranchInitData> BranchesData { get; init; }

        IEnumerable<IActivityInitData> ActivitiesData { get; init; }
    }
}
