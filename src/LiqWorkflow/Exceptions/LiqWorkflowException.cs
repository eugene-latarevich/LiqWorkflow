using System;

namespace LiqWorkflow.Exceptions
{
    public class LiqWorkflowException : Exception
    {
        public LiqWorkflowException()
        {

        }

        public LiqWorkflowException(string message) : base(message)
        {

        }
    }
}
