using System;

namespace LiqWorkflow.Abstractions.Models
{
    public class OnLogData
    {
        public OnLogData(string message, ActivityData data)
        {
            Message = message;
            Data = data;
        }

        public OnLogData(string message, Exception exception)
        {
            IsError = true;
            Message = message;
            Exception = exception;
        }

        public bool IsError { get; }

        public string Message { get; }

        public ActivityData Data { get; }

        public Exception Exception { get; }

        public static OnLogData Info(string message, ActivityData data) => new OnLogData(message, data);

        public static OnLogData Error(string message, Exception exception) => new OnLogData(message, exception);
    }
}
