using System.Collections.Generic;
using LiqWorkflow.Abstractions.Activities;

namespace LiqWorkflow.InitData
{
    public class ActivityInitData : IActivityInitData
    {
        public object ActivityKey { get; init; }

        public object ActivityActionKey { get; init; }

        public object RestoredActivityKey { get; init; }

        public IActivityConfiguration Configuration { get; init; }

        public IEnumerable<string> BranchIds { get; init; }
    }
}
