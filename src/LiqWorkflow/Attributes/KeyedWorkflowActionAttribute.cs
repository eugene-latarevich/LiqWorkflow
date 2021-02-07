using System;

namespace LiqWorkflow.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class KeyedWorkflowActionAttribute : Attribute
    {
        public KeyedWorkflowActionAttribute(object key)
        {
            Key = key;
        }

        public object Key { get; }
    }
}
