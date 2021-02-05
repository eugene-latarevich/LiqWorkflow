using System;
using LiqWorkflow.Abstractions.Branches;

namespace LiqWorkflow.InitData
{
    public class BranchInitData : IBranchInitData
    {
        public BranchInitData(Type type, IBranchConfiguration configuration)
        {
            Type = type;
            Configuration = configuration;
        }

        public Type Type { get; }

        public IBranchConfiguration Configuration { get; }
    }
}
