namespace LiqWorkflow.Exceptions
{
    public class NegativeValueException : LiqWorkflowException
    {
        public NegativeValueException()
            : base("Value cannot be negative")
        {

        }
    }
}
