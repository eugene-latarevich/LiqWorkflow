using System;

namespace LiqWorkflow.Abstractions.Models.Settings
{
    public class RetrySetting
    {
        public int RetryCount { get; init; } = 1;

        public TimeSpan Delay { get; init; } = TimeSpan.FromSeconds(30);
    }
}
