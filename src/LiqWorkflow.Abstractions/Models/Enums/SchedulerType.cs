namespace LiqWorkflow.Abstractions.Models.Enums
{
    public enum SchedulerType
    {
        // Starts immediately once
        Immidiate = 1,

        // Executes once in specific time
        Scheduled = 2,

        // Executes recurrently by chrone or specific time
        Recurring = 3,
    }
}
