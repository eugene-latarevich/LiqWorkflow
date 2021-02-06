using System;
using System.Collections.Generic;
using System.Linq;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;

namespace LiqWorkflow.Abstractions.Models.Builder
{
    public class CreatingBranchConfiguration
    {
        private readonly List<object> _parameters = new List<object>();

        public CreatingBranchConfiguration()
        {

        }

        public Type Type { get; set; }

        public IBranchConfiguration Configuration { get; set; }

        public object[] GetConstructorParameters(IWorkflowConfiguration workflowConfiguration, IEnumerable<IWorkflowActivity> activities)
        {
            _parameters.Add(Configuration);
            _parameters.Add(workflowConfiguration);
            _parameters.Add(activities.ToDictionary(x => x.Configuration.ActivityId));

            return _parameters.ToArray();
        }
    }
}
