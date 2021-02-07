using System;

namespace LiqWorkflow.Abstractions.Models.Containers
{
    public class TypeConfiguration
    {
        public object Key { get; set; }

        public Type Service { get; set; }

        public Type Implementation { get; set; }
    }
}
