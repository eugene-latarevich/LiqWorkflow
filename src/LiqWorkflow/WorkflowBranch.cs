using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Common.Extensions;

namespace LiqWorkflow
{
    // TODO
    // get and store initial data
    // get and store results
    public class WorkflowBranch : IWorkflowBranch
    {
        public WorkflowBranch(ImmutableDictionary<string, IWorkflowActivity> activities)
        {
            Activities = activities;
        }

        public ImmutableDictionary<string, IWorkflowActivity> Activities { get; }

        public async Task PulseAsync(CancellationToken cancellationToken)
        {
            foreach (var keyActivityPair in Activities)
            {
                var activity = keyActivityPair.Value;

                var result = await activity.ExecuteAsync(null, cancellationToken);
            }
        }

        public bool IsValid()
        {
            var activities = Activities.Select(x => x.Value);
            return activities.ValidateBranchStartEnd() && activities.ValidateInnerBranches();
        }
    }
}
