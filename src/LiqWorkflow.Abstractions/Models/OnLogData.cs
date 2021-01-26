namespace LiqWorkflow.Abstractions.Models
{
    public class OnLogData
    {
        public OnLogData(bool isError, string message, ActivityData data)
        {
            IsError = isError;
            Message = message;
            Data = data;
        }

        public bool IsError { get; }

        public string Message { get; set; }

        public ActivityData Data { get; set; }
    }
}
