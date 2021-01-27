using System;
using LiqWorkflow.Abstractions.Models;

namespace LiqWorkflow.Abstractions.Events
{
    public interface IWorkflowMessageEventBroker : IObservable<OnLogData>
    {
        void PublishMessage(OnLogData data);
    }
}
