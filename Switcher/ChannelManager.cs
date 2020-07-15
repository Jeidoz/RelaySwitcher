using Switcher.Data;
using Switcher.Models;
using System.Collections;
using System.Net;
using System.Net.Sockets;

namespace Switcher
{
    public sealed class ChannelManager
    {
        public static int DefaultTcpPort = 60001;

        private readonly UdpClient _client;
        private readonly Config _config;
        private readonly RelayCommand _command;

        //Creates an IPEndPoint to record the IP Address and port number of the sender.
        // The IPEndPoint will allow you to read datagrams sent from any source.
        private IPEndPoint _remoteIpEndPoint;

        public ChannelManager(Config config)
        {
            _config = config;
            _client = new UdpClient(_config.Ip, _config.Port);
            _client.Client.ReceiveTimeout = 5000;
            _client.Client.SendTimeout = 5000;
            _command = new RelayCommand(_config.Password);
            _remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }

        public void SendRelayChannelsSetCommand(Channels channel)
        {
            byte[] data = _command.GetRelayChannelsSetCommand(channel);
            _client.Send(data, data.Length);
        }

        public BitArray GetRelayChannelsStatus()
        {
            byte[] data = _command.GetRelayChannelsStatusCommand();
            _client.Send(data, data.Length);

            byte[] response = _client.Receive(ref _remoteIpEndPoint);
            return new BitArray(new[] { response[RelayCommand.StatusResultByte] });
        }
    }
}