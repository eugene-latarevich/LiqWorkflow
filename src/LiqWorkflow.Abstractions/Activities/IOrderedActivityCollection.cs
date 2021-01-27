using System.Collections.Generic;

namespace LiqWorkflow.Abstractions.Activities
{
    public interface IOrderedActivityCollection : IEnumerable<IWorkflowActivity>
    {
        IOrderedActivityCollection Clone();

        IOrderedActivityCollection StartFrom(string activityId);
    }
}
