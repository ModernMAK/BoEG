using System;
using System.Collections.Generic;

namespace Framework.Core.Networking.MLAPI
{
    public class MessageEventList
    {
        public MessageEventList()
        {
            _eventList = new Dictionary<int, EventHandler<Message>>();
        }

        private readonly Dictionary<int, EventHandler<Message>> _eventList;


        public void Invoke(Message message)
        {
            if (_eventList.TryGetValue(message.Code, out var events))
                events.Invoke(this, message);
        }

        public void Register(int code, EventHandler<Message> listener)
        {
            if (!_eventList.ContainsKey(code))
                _eventList[code] = default;
            _eventList[code] += listener;
        }

        public void Unregister(int code, EventHandler<Message> listener)
        {
            if (_eventList.ContainsKey(code))
                _eventList[code] -= listener;
        }
    }
}