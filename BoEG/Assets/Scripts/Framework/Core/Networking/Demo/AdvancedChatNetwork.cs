using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using MobaGame.Framework.Core.Networking.LLAPI;
using MobaGame.Framework.Core.Networking.MLAPI;
using MobaGame.Framework.Core.Networking.StreamTypes;
using MobaGame.Framework.Core.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame.Framework.Core.Networking.Demo
{
    public class AdvancedChatNetwork : MonoBehaviour
    {
        public enum NetCode
        {
            RawMessage,
            GuidNotification,
            UpdateUsername,
            UserMessage,
        }
#pragma warning disable 0649
        [SerializeField] private InputField _input;
        private List<Tuple<Guid, string>> _messages;
#pragma warning restore 0649

        public const string DefaultHost = "127.0.0.1";
        public const int DefaultPort = SimpleChatNetwork.DefaultPort + 1; //To avoid connections from similar apps


        private IEnumerable<Tuple<Guid, string>> LastMessages(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var j = (_messages.Count - 1) - (count - i - 1);
                if (j >= 0 && j < _messages.Count)
                    yield return _messages[j];
            }
        }

        private string MessagesToString(IEnumerable<Tuple<Guid, string>> messages)
        {
            var builder = new StringBuilder();
            foreach (var message in messages)
            {
                var guid = message.Item1;

                if (!_userNames.TryGetValue(guid, out var userName))
                {
                    if (guid == Guid.Empty)
                        userName = "SERVER";
                    else if (guid == _myId)
                        userName = "LOCAL";
                    else
                        userName = guid.ToString("N");
                }

                var text = message.Item2;

                builder.AppendLine($"[{userName}]: {text}");
            }

            return builder.ToString();
        }

        private Guid _myId;
        private Dictionary<Guid, string> _userNames;

        public static IPEndPoint DefaultEndPoint
        {
            get
            {
                var address = IPAddress.Parse(DefaultHost);
                return new IPEndPoint(address, DefaultPort);
            }
        }


        private NetworkServer _networkServer;
        private MessageServer _messageServer;
        private NetworkClient _networkClient;
        private MessageClient _messageClient;

        public bool IsClient => _networkClient != null;
        public bool IsServer => _networkServer != null;
        public bool IsOnline => IsClient || IsServer;

        public Text _text;


        public void StartClient() => StartClient(DefaultEndPoint);

        public void StartClient(IPEndPoint serverInfo)
        {
            if (IsOnline)
            {
                _messages.Add(new Tuple<Guid, string>(Guid.Empty, "[Cannot Start Client While Online]"));
                UpdateText();
                return;
            }

            _networkClient = new NetworkClient();
            _messageClient = new MessageClient(_networkClient);
            _messageClient.Started += ClientStarted;
            _messageClient.Stopped += ClientStopped;
            _messageClient.Received.Register(NetCode.RawMessage, ClientOnRawMessageReceived);
            _messageClient.Received.Register(NetCode.UpdateUsername, ClientOnUpdateUsernameRecieved);
            _messageClient.Received.Register(NetCode.GuidNotification, ClientOnGuidNotificationReceived);
            _messageClient.Received.Register(NetCode.UserMessage, ClientOnUserMessageReceived);
            _messageClient.Sent.Register(NetCode.UpdateUsername, ClientOnUpdateUsernameSent);
            _messageClient.Sent.Register(NetCode.RawMessage, ClientOnMessageSent);
            _messageClient.Sent.Register(NetCode.UserMessage, ClientOnMessageSent);
            if (!_networkClient.TryConnect(serverInfo))
            {
                _messages.Add(new Tuple<Guid, string>(Guid.Empty, "[Client Failed To Connect]"));
                UpdateText();
                _networkClient.Close();
                _networkClient = null;
            }
        }


        
        private void ClientOnUpdateUsernameSent(object sender, Message e)
        {
            e.Stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new BinaryReader(e.Stream))
            {
                var deserializer = new BinaryDeserializer(reader);
                var userName = deserializer.ReadString();
                _userNames[_myId] = userName;
            }
            UpdateText();
        }

        private void ClientOnUserMessageReceived(object sender, Message e)
        {
            e.Stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new BinaryReader(e.Stream))
            {
                var deserializer = new BinaryDeserializer(reader);
                var guidStr = deserializer.ReadString();
                var message = deserializer.ReadString();
                var guid = Guid.Parse(guidStr);
                _messages.Add(new Tuple<Guid, string>(guid, message));
            }

            UpdateText();
        }

        private void ClientOnGuidNotificationReceived(object sender, Message e)
        {
            e.Stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new BinaryReader(e.Stream))
            {
                var deserializer = new BinaryDeserializer(reader);
                var guidStr = deserializer.ReadString();
                var guid = Guid.Parse(guidStr);
                _myId = guid;
            }

            UpdateText();
        }


        private void Awake()
        {
            _userNames = new Dictionary<Guid, string>();
            _messages = new List<Tuple<Guid, string>>();
            _myId = Guid.Empty;
        }

        private void ClientOnUpdateUsernameRecieved(object sender, Message e)
        {
            e.Stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new BinaryReader(e.Stream))
            {
                var deserializer = new BinaryDeserializer(reader);
                var items = deserializer.ReadInt();
                for (var i = 0; i < items; i++)
                {
                    var guidStr = deserializer.ReadString();
                    var userName = deserializer.ReadString();
                    var guid = Guid.Parse(guidStr);
                    _userNames[guid] = userName;
                }
            }

            UpdateText();
        }

        private void ClientOnMessageSent(object sender, Message message)
        {
            message.Stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(message.Stream))
            {
                var msg = reader.ReadToEnd();
                _messages.Add(new Tuple<Guid, string>(_myId, msg));
            }

            UpdateText();
        }

        private void UpdateText()
        {
            if (_text != null)
                _text.text = MessagesToString(LastMessages(20));
        }

        private void ClientStarted(object sender, EventArgs eventArgs)
        {
            _messages.Add(new Tuple<Guid, string>(_myId, "Connected To Server"));
            UpdateText();
        }

        private void ClientStopped(object sender, EventArgs eventArgs)
        {
            _messages.Add(new Tuple<Guid, string>(_myId, "Disconnected From Server"));
            UpdateText();
        }

        private void ClientOnRawMessageReceived(object sender, Message message)
        {
            message.Stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(message.Stream, Encoding.ASCII))
            {
                var msg = reader.ReadToEnd();
                _messages.Add(new Tuple<Guid, string>(Guid.Empty, msg));
            }

            UpdateText();
        }

        public void StartServer() => StartServer(DefaultEndPoint);

        public void StartServer(IPEndPoint socketInfo, int maxRequests = 0)
        {
            if (IsOnline)
            {
                _messages.Add(new Tuple<Guid, string>(Guid.Empty, "Cannot Start Server While Online"));
                UpdateText();
                return;
            }

            _networkServer = new NetworkServer(socketInfo);
            _messageServer = new MessageServer(_networkServer);
            _messageServer.Started += ServerOnServerStarted;
            _messageServer.Stopped += ServerOnSeverStopped;
            _messageServer.ClientConnected += ServerOnClientConnected;
            _messageServer.ClientDisconnected += ServerOnClientDisconnected;
            _messageServer.ReceivedFrom.Register(NetCode.UserMessage, ServerOnUserMessageReceived);
            _messageServer.ReceivedFrom.Register(NetCode.RawMessage, ServerOnUserMessageReceived);
            _messageServer.ReceivedFrom.Register(NetCode.UpdateUsername, ServerOnUpdateUserNameReceived);
            // _messageServer.SentTo.Register(NetCode.GuidNotification, ServerOnGuidNotifcationSent);
            // _messageServer.Sent.Register(NetCode.UpdateUsername, ServerOnUpdateUsernameSent);
            if (maxRequests <= 0)
                _messageServer.Start();
            else
                _messageServer.Start(maxRequests);
        }



        private void ServerOnUpdateUserNameReceived(object sender, MessageSenderEventArgs e)
        {
            e.Message.Stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new BinaryReader(e.Message.Stream))
            {
                var deserializer = new BinaryDeserializer(reader);
                var userName = deserializer.ReadString();
                UpdateUsername(e.Sender, userName);
            }
            
            using(var memStream = new MemoryStream())
            using (var writer = new BinaryWriter(memStream))
            {
                writer.Write(_userNames.Count);
                foreach (var kvp in _userNames)
                {
                    writer.Write(kvp.Key.ToString());
                    writer.Write(kvp.Value);
                }
                var msg = new Message(NetCode.UpdateUsername, memStream);
                _messageServer.WriteMessageAll(msg);
            }
            UpdateText();
        }

        private void UpdateUsername(Guid eSender, string userName)
        {
            _userNames[eSender] = userName;
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                var msg = new Message(NetCode.UpdateUsername, stream);

                var serializer = new BinarySerializer(writer);
                if (IsServer)
                {
                    serializer.Write(eSender.ToString());
                    serializer.Write(userName);
                    _messageServer.WriteMessageRelay(eSender, msg);
                }
                else if (IsClient)
                {
                    serializer.Write(userName);
                    _messageClient.WriteMessage(msg);
                }
            }
        }


        private void ServerOnClientConnected(object sender, Guid guid)
        {
            _messages.Add(new Tuple<Guid, string>(guid, "[Client Connected]"));
            
            UpdateText();
            
            using (var memStream = new MemoryStream())
            using (var writer = new BinaryWriter(new NonClosingStream(memStream)))
            {
                writer.Write(guid.ToString());
                var newMessage = new Message(NetCode.GuidNotification, memStream);
                _messageServer.WriteMessage(guid, newMessage);
            }
        }

        private void ServerOnClientDisconnected(object sender, Guid guid)
        {
            _messages.Add(new Tuple<Guid, string>(guid, "[Client Disconnected]"));
            UpdateText();

        }

        private void ServerOnUserMessageReceived(object sender, MessageSenderEventArgs e)
        {
            var guid = e.Sender; //.Item1;
            var stream = e.Message.Stream; // tuple.Item2;

            stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(stream, Encoding.ASCII))
            {
                stream.Seek(0, SeekOrigin.Begin);
                var msg = reader.ReadLine();
                _messages.Add(new Tuple<Guid, string>(guid, msg));
                UpdateText();

                using (var memStream = new MemoryStream())
                using (var writer = new BinaryWriter(new NonClosingStream(memStream)))
                {
                    writer.Write(guid.ToString());
                    writer.Write(msg);
                    var newMessage = new Message(NetCode.UserMessage, memStream);
                    _messageServer.WriteMessageRelay(guid, newMessage);
                }
            }


            stream.Seek(0, SeekOrigin.Begin);
            _networkServer.WriteMessageRelay(guid, stream);
        }


        private void ServerOnServerStarted(object sender, EventArgs eventArgs)
        {
            _messages.Add(new Tuple<Guid, string>(Guid.Empty, "Started Server"));
            UpdateText();
        }

        private void ServerOnSeverStopped(object sender, EventArgs eventArgs)
        {
            _messages.Add(new Tuple<Guid, string>(Guid.Empty, "Closed Server"));
            UpdateText();
        }

        public void Stop()
        {
            if (_networkServer != null)
            {
                _networkServer.Stop();
                _messageServer = null;
                _networkServer = null;
            }

            if (_networkClient != null)
            {
                _networkClient.Close();
                _messageClient = null;
                _networkClient = null;
            }
        }


        public void SendUserMessage()
        {
            if (!IsOnline)
            {
                _messages.Add(new Tuple<Guid, string>(Guid.Empty, "Not Online!"));
                UpdateText();
                return;
            }

            var chatText = _input.text;
            _input.text = "";
            if (chatText == "")
            {
                _messages.Add(new Tuple<Guid, string>(Guid.Empty, "Cannot Send Blank Message!"));
                UpdateText();
                return;
            }

            using (var memory = new MemoryStream())
            {
                using (var writer = new BinaryWriter(new NonClosingStream(memory)))
                {
                    if (IsServer)
                        writer.Write(Guid.Empty.ToString()); //Clients expect a user, server does not
                    writer.Write(chatText);
                    memory.Seek(0, SeekOrigin.Begin);
                    var message = new Message(NetCode.UserMessage, memory);
                    if (IsClient)
                        _messageClient.WriteMessage(message);
                    else if (IsServer)
                    {
                        _messageServer.WriteMessageAll(message);
                        _messages.Add(new Tuple<Guid, string>(Guid.Empty, chatText));
                        UpdateText();
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (_networkServer != null)
                _networkServer.Stop();
            if (_networkClient != null)
                _networkClient.Close();
        }

        private void Update()
        {
            if (IsServer)
            {
                _networkServer.TryAcceptClient(out _, out _);
                _messageServer.ReadAllMessages();
                _networkServer.DropDisconnectedClients();
            }

            else if (IsClient)
            {
                _messageClient.ReadAllMessages();
                if (!_networkClient.CheckServer())
                    Stop();
            }
        }
    }
}