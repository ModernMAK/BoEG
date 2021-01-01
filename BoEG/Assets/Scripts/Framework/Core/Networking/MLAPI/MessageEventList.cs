using System;
using System.Collections.Generic;
using UnityEngine.Networking.PlayerConnection;

namespace Framework.Core.Networking.MLAPI
{
    public class EventHandlerList<TArgs>
    {
        public EventHandlerList()
        {
            _eventList = new Dictionary<int, EventHandler<TArgs>>();
        }

        private readonly Dictionary<int, EventHandler<TArgs>> _eventList;


        public void Invoke(int code, TArgs args)
        {
            if (_eventList.TryGetValue(code, out var events))
                events.Invoke(this, args);
        }

        public void Register(IConvertible code, EventHandler<TArgs> listener) =>
            Register(Convert.ToInt32(code), listener);

        public void Register(int code, EventHandler<TArgs> listener)
        {
            if (!_eventList.ContainsKey(code))
                _eventList[code] = default;
            _eventList[code] += listener;
        }

        public void Unregister(IConvertible code, EventHandler<TArgs> listener) =>
            Unregister(Convert.ToInt32(code), listener);

        public void Unregister(int code, EventHandler<TArgs> listener)
        {
            if (_eventList.ContainsKey(code))
                _eventList[code] -= listener;
        }
    }

    public class MessageEventList : EventHandlerList<Message>
    {
        public void Invoke(Message message) => base.Invoke(message.Code, message);
    }

    public class MessageSenderEventArgs : EventArgs
    {
        public MessageSenderEventArgs(Guid guid, Message message)

        {
            Message = message;
            Sender = guid;
        }

        public Message Message { get; }
        public Guid Sender { get; }
    }

    public class MessageSenderEventList : EventHandlerList<MessageSenderEventArgs>
    {
        public void Invoke(MessageSenderEventArgs args) => base.Invoke(args.Message.Code, args);
    }
}