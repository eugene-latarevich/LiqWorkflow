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

    public class WorkflowResult<TData> : WorkflowResult
    {
        public WorkflowResult(bool succeeded, TData value, Exception exception = null)
            : base(succeeded, exception)
        {
            Value = value;
        }

        public TData Value { get; }

        public static WorkflowResult Ok(TData value) => new WorkflowResult<TData>(true, value);

        public new static WorkflowResult<TData> Error(Exception exception = null) => new WorkflowResult<TData>(false, default, exception);
    }
}
