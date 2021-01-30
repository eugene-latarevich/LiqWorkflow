using System.Collections.Generic;
using LiqWorkflow.Abstractions.Models.Factories;

namespace LiqWorkflow.Abstractions.Factories
{
    public interface IWorkflowBranchFactory
    {
        IEnumerable<IWorkflowBranch> BuildConnected(ConnectedBranchesConfiguration configuration);
    }
}
