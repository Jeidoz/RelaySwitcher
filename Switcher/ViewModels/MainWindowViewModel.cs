using Switcher.Data.Enums;
using Switcher.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Switcher.ViewModels
{
    public sealed class MainWindowViewModel : BaseViewModel
    {
        private const int MaxRelayChannels = 8;

        private Config _appConfig;
        private ObservableCollection<RelayButton> _switchButtons;

        public MainWindowViewModel(string configPath)
        {
            if (!File.Exists(configPath))
            {
                AppConfig = Config.Default;
                AppConfig.SaveToFile(configPath);
            }
            else
            {
                AppConfig = Config.LoadFromFile(configPath);
            }
        }

        private void UpdateEnableSwitchers()
        {
            int maxSwitchButtonIndex = _appConfig.RelayType switch
            {
                RelayType.FourChannels => 4,
                RelayType.SixChannels => 6,
                RelayType.EightChannels => 8,
                _ => throw new ArgumentOutOfRangeException()
            };
            int i = 0;
            for (; i < maxSwitchButtonIndex; ++i)
            {
                SwitchButtons[i].IsVisible = true;
            }

            for (; i < MaxRelayChannels; ++i)
            {
                SwitchButtons[i].IsVisible = false;
            }
        }

        public ObservableCollection<RelayButton> SwitchButtons
        {
            get => _switchButtons;
            set
            {
                _switchButtons = value;
                UpdateEnableSwitchers();
                OnPropertyChanged(nameof(SwitchButtons));
            }
        }

        public Config AppConfig
        {
            get => _appConfig;
            set
            {
                _appConfig = value;
                if (SwitchButtons != null)
                {
                    UpdateEnableSwitchers();
                }

                OnPropertyChanged(nameof(AppConfig));
            }
        }
    }
}