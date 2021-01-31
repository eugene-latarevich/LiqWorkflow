﻿using System;
using LiqWorkflow.Abstractions.Branches;

namespace LiqWorkflow.Abstractions.Models.Builder
{
    public class CreatingBranchConfiguration
    {
        public Type Type { get; set; }

        public IBranchConfiguration Configuration { get; set; }
    }
}
