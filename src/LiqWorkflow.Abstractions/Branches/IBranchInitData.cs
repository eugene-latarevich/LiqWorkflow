using System;

namespace LiqWorkflow.Abstractions.Branches
{
    public interface IBranchInitData
    {
        Type Type { get; }

        IBranchConfiguration Configuration { get; }
    }
}
