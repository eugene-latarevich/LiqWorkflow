using System.Collections.Generic;

namespace LiqWorkflow.Abstractions.Activities
{
    public interface IActivityInitData
    {
        object ActivityKey { get; init; }

        object ActivityActionKey { get; init; }

        object RestoredActivityKey { get; init; }

        IActivityConfiguration Configuration { get; init; }

        IEnumerable<string> BranchIds { get; init; }
    }
}
