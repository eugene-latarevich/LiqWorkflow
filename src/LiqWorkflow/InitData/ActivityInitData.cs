using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using LiqWorkflow.Abstractions.Activities;

namespace LiqWorkflow.InitData
{
    public class ActivityInitData : IActivityInitData
    {
        private readonly List<object> _parameters = new List<object>();

        public ActivityInitData(
            Type activityType,
            Type executableActivityType, 
            IEnumerable<string> branchIds, 
            IActivityConfiguration configuration)
        {
            ActivityType = activityType;
            ExecutableActivityType = executableActivityType;
            Configuration = configuration;
            BranchIds = branchIds;
        }

        public Type ActivityType { get; }

        public Type ExecutableActivityType { get; }

        public IActivityConfiguration Configuration { get; }

        public IEnumerable<string> BranchIds { get; }

        public IEnumerable<object> Parameters => _parameters.ToImmutableList();

        public IActivityInitData WithParameter(object parameter)
        {
            _parameters.Add(parameter);

            return this;
        }
    }
}
