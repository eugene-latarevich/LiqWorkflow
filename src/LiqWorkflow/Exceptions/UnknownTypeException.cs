namespace LiqWorkflow.Exceptions
{
    public class UnknownTypeException : LiqWorkflowException
    {
        public UnknownTypeException()
            : base("Unknown type")
        {

        }

        public UnknownTypeException(string message)
            : base(message)
        {

        }
    }
}
