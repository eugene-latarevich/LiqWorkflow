using System.Collections.Generic;

namespace LiqWorkflow.Abstractions.Activities
{
    public interface IActivityInitData
    {
        object ActivityKey { get; }

        object ActivityActionKey { get; }

        object RestoredActivityKey { get; }

        IActivityConfiguration Configuration { get; }

        IEnumerable<string> BranchIds { get; }
    }
}
