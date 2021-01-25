using System;

namespace LiqWorkflow.Abstractions.Models
{
    public class WorkflowResult
    {
        public WorkflowResult(bool succeeded, Exception exception = null)
        {
            Succeeded = succeeded;
            Exception = exception;
        }

        public bool Succeeded { get; }

        public Exception Exception { get; }

        public static WorkflowResult Ok() => new WorkflowResult(true);

        public static WorkflowResult Error(Exception exception = null) => new WorkflowResult(false, exception);
    }
}
