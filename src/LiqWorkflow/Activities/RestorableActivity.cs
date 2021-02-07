using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Branches;
using LiqWorkflow.Abstractions.Models;

namespace LiqWorkflow.Activities
{
    public abstract class RestorableActivity : Activity, IRestorableWorkflowActivitity
    {
        protected RestorableActivity(
            IWorkflowExecutableAction action,
            IActivityConfiguration configuration,
            IDictionary<string, IWorkflowBranch> branches)
            : base(action, configuration, branches)
        {

        }

        public async Task<WorkflowResult<ActivityData>> RestoreAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var restoredData = await FindRestoredDataAsync(Configuration, cancellationToken);
                return await ExecuteAsync(restoredData, cancellationToken);
            }
            catch (Exception exception)
            {
                return ProcessErrorResult(exception);
            }
        }

        protected abstract Task<ActivityData> FindRestoredDataAsync(IActivityConfiguration configuration, CancellationToken cancellationToken = default);
    }
}
