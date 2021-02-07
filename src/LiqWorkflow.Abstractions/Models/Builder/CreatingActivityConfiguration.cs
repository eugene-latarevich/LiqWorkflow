using System.Collections.Generic;
using System.Linq;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;
using LiqWorkflow.Abstractions.Containers;

namespace LiqWorkflow.Abstractions.Models.Builder
{
    public class CreatingActivityConfiguration
    {
        private readonly IContainer _container;

        public CreatingActivityConfiguration(IContainer container)
        {
            _container = container;
        }

        public object ActiviyKey { get; set; }

        public object ActivityActionKey { get; set; }

        public IActivityConfiguration Configuration { get; set; }

        public IEnumerable<string> BranchIds { get; set; }

        public object[] GetConstructorParameters(IEnumerable<IWorkflowBranch> connectedBranches)
        {
            var parameters = new List<object>();

            parameters.Add(Configuration);
            parameters.Add(_container.GetKeyedService<IWorkflowExecutableAction>(ActivityActionKey));
            parameters.Add(connectedBranches.ToDictionary(x => x.Configuration.BranchId));

            return parameters.ToArray();
        }
    }
}
