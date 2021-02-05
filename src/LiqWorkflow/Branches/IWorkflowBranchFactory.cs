using System.Collections.Generic;
using LiqWorkflow.Abstractions.Branches;
using LiqWorkflow.Abstractions.Models.Factories;

namespace LiqWorkflow.Branches
{
    public interface IWorkflowBranchFactory
    {
        IEnumerable<IWorkflowBranch> BuildConnected(ConnectedBranchesConfiguration configuration);
    }
}
