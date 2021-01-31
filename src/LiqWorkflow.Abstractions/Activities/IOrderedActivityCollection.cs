using System.Collections.Generic;

namespace LiqWorkflow.Abstractions.Activities
{
    public interface IOrderedActivityCollection : IEnumerable<IWorkflowExecutableActivity>
    {
        IOrderedActivityCollection Clone();

        IOrderedActivityCollection StartFrom(string activityId);
    }
}
