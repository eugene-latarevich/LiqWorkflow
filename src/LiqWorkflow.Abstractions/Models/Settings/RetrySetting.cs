using System;

namespace LiqWorkflow.Abstractions.Models.Settings
{
    public class RetrySetting
    {
        public int RetryCount { get; } = 1;

        public TimeSpan Delay { get; } = TimeSpan.FromSeconds(30);
    }
}
