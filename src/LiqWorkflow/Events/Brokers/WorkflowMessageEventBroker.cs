using System;
using System.Collections.Generic;
using System.Linq;
using LiqWorkflow.Abstractions.Events;
using LiqWorkflow.Abstractions.Models;

namespace LiqWorkflow.Events.Brokers
{
    class WorkflowMessageEventBroker : IWorkflowMessageEventBroker
    {
        private readonly object _lockObject = new object();

        private List<IObserver<OnLogData>> _observers;

        public WorkflowMessageEventBroker()
        {

        }

        public IDisposable Subscribe(IObserver<OnLogData> observer)
        {
            lock (_lockObject)
            {
                _observers ??= new List<IObserver<OnLogData>>();
                if (_observers.Any(existingObsever => observer.Equals(existingObsever)))
                {
                    throw new ArgumentException("Observer already exists.");
                }

                _observers.Add(observer);

                return new WorkflowEventUnsubscriber<OnLogData>(observer, _observers);
            }
        }

        public void PublishMessage(OnLogData data)
        {
            lock (_lockObject)
            {
                _observers.ForEach(observer => observer.OnNext(data));
            }
        }
    }
}
