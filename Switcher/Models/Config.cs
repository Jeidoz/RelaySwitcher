using Newtonsoft.Json;
using Switcher.ViewModels;
using System.IO;

namespace Switcher.Models
{
    public sealed class Config : BaseViewModel
    {
        public static readonly string FileName = "config.json";

        private const float DefaultPauseInSeconds = 2;

        private string _ip;
        private int _port;
        private int _password;
        private float _pauseBetweenRequests;

        public string Ip
        {
            get => _ip;
            set
            {
                _ip = value;
                OnPropertyChanged(nameof(Ip));
            }
        }
        public int Port
        {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged(nameof(Port));
            }
        }
        public int Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public float PauseBetweenRequests
        {
            get => _pauseBetweenRequests;
            set
            {
                _pauseBetweenRequests = value;
                OnPropertyChanged(nameof(PauseBetweenRequests));
            }
        }

        public static readonly Config Default = new Config
        {
            _ip = "0.0.0.0",
            _port = ChannelManager.DefaultTcpPort,
            _password = 0,
            _pauseBetweenRequests = DefaultPauseInSeconds
        };

        public static Config LoadFromFile(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Config>(json);
        }

        public void SaveToFile(string path)
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}