using System;
using System.Collections.Generic;
using System.Linq;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;

namespace LiqWorkflow.Abstractions.Models.Builder
{
    public class CreatingActivityConfiguration
    {
        public Type ActiviyType { get; set; }

        public Type ExecutableActivityType { get; set; }

        public IActivityConfiguration Configuration { get; set; }

        public IEnumerable<string> BranchIds { get; set; }

        public IEnumerable<object> Parameters { get; set; }

        public object[] GetConstructorParameters(IEnumerable<IWorkflowBranch> connectedBranches)
        {
            var parameters = Parameters.ToList();

            parameters.Add(Configuration);
            parameters.Add(Configuration.ServiceProvider.GetService(ExecutableActivityType));
            parameters.Add(connectedBranches.ToDictionary(x => x.Configuration.BranchId));

            return parameters.ToArray();
        }
    }
}
