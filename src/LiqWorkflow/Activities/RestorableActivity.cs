using System.Collections.Generic;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;

namespace LiqWorkflow.Activities
{
    public abstract class RestorableActivity : Activity, IRestorableWorkflowActivitity
    {
        protected RestorableActivity(
            IWorkflowExecutableActivity action,
            IActivityConfiguration configuration,
            IDictionary<string, IWorkflowBranch> branches)
            : base(action, configuration, branches)
        {

        }

        
    }
}
