using System;
using System.Collections.Generic;

namespace LiqWorkflow.Abstractions.Activities
{
    public interface IActivityInitData
    {
        Type ActivityType { get; }

        Type ExecutableActivityType { get; }

        IActivityConfiguration Configuration { get; }

        IEnumerable<string> BranchIds { get; }

        IEnumerable<object> Parameters { get; }

        IActivityInitData WithParameter(object parameter);
    }
}
