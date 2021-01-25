﻿using System.Threading;
using LiqWorkflow.Abstractions.Models.Configurations;

namespace LiqWorkflow.Abstractions
{
    public interface IWorkflowConfiguration
    {
        string Id { get; }

        string Name { get; }

        CancellationTokenSource CancellationTokenSource { get; }

        SchedulerConfiguration SchedulerConfiguration { get; }
    }
}
