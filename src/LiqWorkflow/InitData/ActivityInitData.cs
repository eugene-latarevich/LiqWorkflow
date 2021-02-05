using System;
using LiqWorkflow.Abstractions.Activities;

namespace LiqWorkflow.InitData
{
    public class ActivityInitData : IActivityInitData
    {
        public ActivityInitData(Type type, IActivityConfiguration configuration)
        {
            Type = type;
            Configuration = configuration;
        }

        public Type Type { get; }

        public IActivityConfiguration Configuration { get; }
    }
}
