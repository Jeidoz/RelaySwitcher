using System;
using System.Net.Sockets;
using System.Text;

namespace Switcher
{
    public sealed class ChannelManager
    {
        public static int DefaultTcpPort = 60001;
        
        private const char TurnOn = '1';
        private const string TurnOffAll = "2X";
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        private string _ip;
        private int _port;

        public string Ip
        {
            get => _ip;
            set
            {
                Close();
                _ip = value;
                Open();
            }
        }

        public int Port
        {
            get => _port;
            set
            {
                Close();
                _port = value;
                Open();
            }
        }

        public ChannelManager()
        {
            _ip = "127.0.0.1";
            _port = DefaultTcpPort;
        }
        public ChannelManager(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        private string SendMessage(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            _stream.Write(data, 0, data.Length);

            data = new byte[256];
            int bytes = _stream.Read(data, 0, data.Length);
            return Encoding.ASCII.GetString(data, 0, bytes);
        }

        public string TurnOnChannel(Channels channel)
        {
            return SendMessage($"{TurnOn}{channel:d}");
        }

        public string TurnOffAllChannels()
        {
            return SendMessage(TurnOffAll);
        }

        public void Open()
        {
            _tcpClient.Connect(_ip, _port);
            _stream = _tcpClient.GetStream();
        }

        public void Close()
        {
            _stream.Close();
            _tcpClient.Close();
        }
    }
}