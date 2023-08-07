using System;
using System.Collections.Generic;
using Project.Scripts.Patterns.Singelton;

namespace Project.Scripts.Patterns.Observer
{
    public class EventBus : ThreadSafeSingleton<EventBus>
    {
        public delegate void EventDelegate<T>(T eventData);
        
        private Dictionary<Type, object> _eventListeners;
        protected EventBus() : base()
        {
            _eventListeners = new Dictionary<Type, object>();
        }

        public void Subscribe<T>(EventDelegate<T> listener)
        {
            Type eventType = typeof(T);
            if (_eventListeners.TryGetValue(eventType, out var existingHandler))
            {
                var eventDelegate = (EventDelegate<T>) existingHandler;
                eventDelegate += listener;
                _eventListeners[eventType] = eventDelegate;
            }
            else
            {
                _eventListeners.Add(eventType, listener);
            }
        }

        public void Unsubscribe<T>(EventDelegate<T> listener)
        {
            Type eventType = typeof(T);
            if (_eventListeners.TryGetValue(eventType, out var existingHandler))
            {
                var eventDelegate = (EventDelegate<T>) existingHandler;
                eventDelegate -= listener;
                if (eventDelegate == null)
                    _eventListeners.Remove(eventType);
                else
                {
                    _eventListeners[eventType] = eventDelegate;
                }
            }
        }

        public void Trigger<T>(T eventData)
        {
            Type eventType = typeof(T);
            if (_eventListeners.TryGetValue(eventType, out var existingHandler))
            {
                var eventDelegate = (EventDelegate<T>) existingHandler;
                eventDelegate?.Invoke(eventData);
            }
        }
    }
}