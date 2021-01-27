using System;
using System.Collections.Generic;
using System.Linq;

namespace LiqWorkflow.Events
{
    class WorkflowEventUnsubscriber<TArgs> : IDisposable
    {
        private readonly IObserver<TArgs> _observer;
        private readonly List<IObserver<TArgs>> _observers;

        public WorkflowEventUnsubscriber(IObserver<TArgs> observer, List<IObserver<TArgs>> observers)
        {
            _observer = observer;
            _observers = observers;
        }

        public void Dispose()
        {
            if (_observers.Any(observer => observer.Equals(_observer)))
            {
                _observers.Remove(_observer);
            }
        }
    }
}
