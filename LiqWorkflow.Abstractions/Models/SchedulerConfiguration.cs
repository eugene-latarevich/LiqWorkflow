using System;
using LiqWorkflow.Abstractions.Models.Enums;

namespace LiqWorkflow.Abstractions.Models
{
    public class SchedulerConfiguration
    {
        public SchedulerConfiguration(SchedulerType schedulerType, string chrone, DateTime startAt)
        {
            SchedulerType = schedulerType;
            Chrone = chrone;
            StartAt = startAt;
        }

        public SchedulerType SchedulerType { get; }

        public string Chrone { get; }

        public DateTime StartAt { get; }
    }
}
