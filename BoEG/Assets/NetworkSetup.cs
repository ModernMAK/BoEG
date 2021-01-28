using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using MobaGame.Framework.Core.Networking.Tracking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame
{
    public class NetworkSetup : MonoBehaviour
    {
        public enum SetupMode
        {
            Server,
            ServerClient,
            Client
        }

        [SerializeField] private TMP_InputField _ipAddress;
        [SerializeField] private TMP_InputField _port;
        [SerializeField] private TMP_Dropdown _mode;
        [SerializeField] private Button _start;
        [SerializeField] private Button _stop;
        [SerializeField] private TMP_InputField _maxConnections;

        public MessageManager Manager => MessageManager.Instance;

        private string IpAddressAsStr => _ipAddress.text;
        private string PortAsStr => _port.text;
        private string ConnectionsAsStr => _maxConnections.text;
        private int ModeAsInt => _mode.value;

        public const int DefaultPort = 8675;
        public int Port => PortAsStr != "" ? int.Parse(PortAsStr) : DefaultPort;

        public SetupMode Mode => (SetupMode) ModeAsInt;
        public static readonly IPAddress DefaultIP = IPAddress.Loopback;
        public IPAddress IpAddress => IpAddressAsStr != "" ? IPAddress.Parse(IpAddressAsStr) : DefaultIP;
        public IPEndPoint EndPoint => new IPEndPoint(IpAddress, Port);

        public bool HasConnections => ConnectionsAsStr != ""; //This one is special, no input means dont specify buffer
        public int Connections => int.Parse(ConnectionsAsStr);

        private void Awake()
        {
            _start.onClick.AddListener(OnStartClick);
            _stop.onClick.AddListener(OnStopClick);
            registered = false;
        }

        private void Start()
        {
            if (!registered && Manager.MessageServer != null && Manager.MessageClient != null)
            {
                Manager.MessageServer.Stopped += MessageServerOnStopped;
                Manager.MessageClient.Stopped += MessageClientOnStopped;
                registered = true;
            }
        }

        private bool registered = false;
        private void OnEnable()
        {
            if (!registered && Manager.MessageServer != null && Manager.MessageClient != null)
            {
                Manager.MessageServer.Stopped += MessageServerOnStopped;
                Manager.MessageClient.Stopped += MessageClientOnStopped;
                registered = true;
            }
        }

        private void OnDisable()
        {
            if (registered)
            {
                Manager.MessageServer.Stopped -= MessageServerOnStopped;
                Manager.MessageClient.Stopped -= MessageClientOnStopped;
                registered = false;
            }
            
        }

        private void MessageClientOnStopped(object sender, EventArgs e)
        {
            TryEnable();
        }

        private void MessageServerOnStopped(object sender, EventArgs e)
        {
            TryEnable();
        }

        private void OnStartClick()
        {
            var con = HasConnections ? Connections : -1;
            UnityEngine.Debug.Log($"{EndPoint} @{Mode} w/ '{con}'");
            switch (Mode)
            {
                case SetupMode.Server:
                    StartServer(MessageManager.ServerMode.RemoteHost);
                    break;
                case SetupMode.ServerClient:
                    StartServer(MessageManager.ServerMode.LocalHost);
                    break;
                case SetupMode.Client:
                    StartClient();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            TryDisable();
        }

        void TryDisable()
        {
            _ipAddress.text = EndPoint.Address.ToString();
            _port.text = EndPoint.Port.ToString();

            SetEnabled(false);
        }

        void TryEnable()
        {
            SetEnabled(true);
        }

        void SetEnabled(bool e)
        {
            _ipAddress.enabled = e;
            _port.enabled = e;
            _mode.enabled = e;
            _maxConnections.enabled = e;
            
            _start.enabled = e;
            _stop.enabled = !e;
        }

        private void StartClient()
        {
            Manager.InitializeClient();
            Manager.StartClient(EndPoint);
        }

        private void StartServer(MessageManager.ServerMode mode)
        {
            Manager.InitializeServer(EndPoint, mode);
            if (HasConnections)
                Manager.StartServer(Connections);
            else
                Manager.StartServer();
        }

        private void OnStopClick()
        {
            UnityEngine.Debug.Log($"Server Stopped");
            if (Manager.IsClient)
                Manager.KillClient();
            else if (Manager.IsServer)
                Manager.KillServer();
            TryEnable();
        }
    }
}