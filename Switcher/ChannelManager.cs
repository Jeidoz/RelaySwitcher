using Switcher.Data;
using Switcher.Data.Enums;
using Switcher.Models;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Switcher
{
    public sealed class ChannelManager
    {
        public const int DefaultTcpPort = 60001;

        private Config _config;
        private readonly int FiveSecondsInMiliseconds = (int) TimeSpan.FromSeconds(5).TotalMilliseconds;
        private readonly UdpClient _client;
        private readonly IotRelayCommand _command;

        // Creates an IPEndPoint to record the IP Address and port number of the sender.
        // The IPEndPoint will allow you to read datagrams sent from any source.
        private IPEndPoint _remoteIpEndPoint;

        public ChannelManager(Config config)
        {
            _config = config;
            if (_client is null)
            {
                _client = new UdpClient(_config.Ip, _config.Port)
                {
                    Client =
                    {
                        ReceiveTimeout = FiveSecondsInMiliseconds,
                        SendTimeout = FiveSecondsInMiliseconds
                    }
                };
            }
            else
            {
                _client.Connect(_config.Ip, _config.Port);
            }

            _command = new IotRelayCommand(_config.Password);
            _remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }

        public void SetConfigWithSameSocket(Config newConfig)
        {
            _config = newConfig;
            _command.Password = (ushort)_config.Password;
        }

        public Task SendRelayChannelsSetCommand(Channels channel)
        {
            return Task.Run(() =>
            {
                byte[] data = _command.GetRelayChannelsSetCommand(Channels.None);
                _client.Send(data, data.Length);

                Thread.Sleep(TimeSpan.FromSeconds(_config.PauseBetweenRequests));

                data = _command.GetRelayChannelsSetCommand(channel);
                _client.Send(data, data.Length);
            });
        }

        public BitArray GetRelayChannelsStatus()
        {
            byte[] data = _command.GetRelayChannelsStatusCommand();
            _client.Send(data, data.Length);

            byte[] response = _client.Receive(ref _remoteIpEndPoint);
            return new BitArray(new[] { response[IotRelayCommand.StatusResultByte] });
        }
    }
}