using System;

namespace LiqWorkflow.Abstractions.Activities
{
    public interface IActivityInitData
    {
        Type Type { get; }

        IActivityConfiguration Configuration { get; }
    }
}
