using Newtonsoft.Json;
using Switcher.ViewModels;
using System;
using System.Collections.ObjectModel;
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
        private bool _isTopMost;

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
        public bool IsTopMost
        {
            get => _isTopMost;
            set
            {
                _isTopMost = value;
                OnPropertyChanged(nameof(IsTopMost));
            }
        }
        public ObservableCollection<RelayLabel> RelayLabels { get; set; }

        public static readonly Config Default = new Config
        {
            _ip = "127.0.0.1",
            _port = ChannelManager.DefaultTcpPort,
            _password = 0,
            _pauseBetweenRequests = DefaultPauseInSeconds,
            _isTopMost = false,
            RelayLabels = new ObservableCollection<RelayLabel>
            {
                new RelayLabel(Channels.First, "Relay #1"),
                new RelayLabel(Channels.Second, "Relay #2"),
                new RelayLabel(Channels.Third, "Relay #3"),
                new RelayLabel(Channels.Fourth, "Relay #4"),
                new RelayLabel(Channels.Fifth, "Relay #5"),
                new RelayLabel(Channels.Sixth, "Relay #6"),
                new RelayLabel(Channels.Seventh, "Relay #7"),
                new RelayLabel(Channels.Eighth, "Relay #8")
            }
        };

        public Config()
        {
        }

        public Config(Config config)
        {
            _ip = string.Copy(config._ip);
            _port = config._port;
            _password = config._password;
            _pauseBetweenRequests = config._pauseBetweenRequests;
            IsTopMost = config._isTopMost;
            RelayLabels = new ObservableCollection<RelayLabel>(config.RelayLabels);
        }

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

        public bool IsSameSocketConfig(Config newConfig)
        {
            return _ip == newConfig._ip &&
                   _port == newConfig._port;
        }

        public override bool Equals(object obj)
        {
            const float MinDiff = 1e-9f;
            if ((obj is null) || GetType() != obj.GetType())
            {
                return false;
            }

            var config = (Config)obj;

            return IsSameSocketConfig(config) &&
                   _password == config._password &&
                   Math.Abs(_pauseBetweenRequests - config._pauseBetweenRequests) < MinDiff &&
                   _isTopMost == config._isTopMost;
        }
    }
}