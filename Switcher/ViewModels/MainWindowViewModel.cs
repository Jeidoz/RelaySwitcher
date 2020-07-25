using Switcher.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace Switcher.ViewModels
{
    public sealed class MainWindowViewModel : BaseViewModel
    {
        private Config _appConfig;

        public MainWindowViewModel(string configPath)
        {
            if (!File.Exists(configPath))
            {
                _appConfig = Config.Default;
                _appConfig.SaveToFile(configPath);
            }
            else
            {
                _appConfig = Config.LoadFromFile(configPath);
            }
        }

        public ObservableCollection<RelayButton> SwitchButtons { get; set; }

        public Config AppConfig
        {
            get => _appConfig;
            set
            {
                _appConfig = value;
                OnPropertyChanged(nameof(AppConfig));
            }
        }
    }
}