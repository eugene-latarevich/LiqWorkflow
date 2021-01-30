using System;
using System.Collections.Generic;
using LiqWorkflow.Abstractions.Activities;

namespace LiqWorkflow.Abstractions.Models.Builder
{
    public class CreatingActivityConfiguration
    {
        public Type Type { get; set; }

        public IActivityConfiguration Configuration { get; set; }

        public List<string> BranchIds { get; set; }
    }
}
