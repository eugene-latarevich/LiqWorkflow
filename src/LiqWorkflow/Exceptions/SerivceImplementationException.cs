using System;

namespace LiqWorkflow.Exceptions
{
    public class SerivceImplementationException : LiqWorkflowException
    {
        public SerivceImplementationException(Type type) 
            : base($"Cannot implement service with Type={type}")
        {

        }
    }
}
