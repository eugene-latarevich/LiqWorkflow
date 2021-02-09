using System.Collections.Generic;
using LiqWorkflow.Abstractions.Activities;

namespace LiqWorkflow.InitData
{
    public class ActivityInitData : IActivityInitData
    {
        private readonly List<object> _parameters = new List<object>();

        public ActivityInitData(
            object activityKey,
            object activityActionKey, 
            object restoredActivityKey,
            IEnumerable<string> branchIds, 
            IActivityConfiguration configuration)
        {
            ActivityKey = activityKey;
            ActivityActionKey = activityActionKey;
            RestoredActivityKey = restoredActivityKey;
            Configuration = configuration;
            BranchIds = branchIds;
        }

        public object ActivityKey { get; }

        public object ActivityActionKey { get; }

        public object RestoredActivityKey { get; }

        public IActivityConfiguration Configuration { get; }

        public IEnumerable<string> BranchIds { get; }
    }
}
