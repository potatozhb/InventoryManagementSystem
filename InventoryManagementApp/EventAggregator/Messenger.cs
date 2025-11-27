using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementApp.EventAggregator
{
    public class Messenger
    {
        private static Messenger? _instance;
        public static Messenger Instance => _instance ??= new Messenger();

        private readonly Dictionary<Type, List<Action<object>>> _subscribers = new();

        public void Subscribe<TMessage>(Action<TMessage> action)
        {
            var messageType = typeof(TMessage);
            if (!_subscribers.ContainsKey(messageType))
                _subscribers[messageType] = new List<Action<object>>();
            _subscribers[messageType].Add(o => action((TMessage)o));
        }

        public void Unsubscribe<TMessage>(Action<TMessage> action)
        {
            var messageType = typeof(TMessage);
            if (_subscribers.ContainsKey(messageType))
            {
                _subscribers[messageType].RemoveAll(a => a.Equals((Action<object>)(o => action((TMessage)o))));
            }
        }

        public void Publish<TMessage>(TMessage message)
        {
            var messageType = typeof(TMessage);
            if (_subscribers.ContainsKey(messageType))
            {
                foreach (var action in _subscribers[messageType])
                    action(message!);
            }
        }
    }
}
