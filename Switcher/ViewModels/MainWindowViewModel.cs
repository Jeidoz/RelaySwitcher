using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using Switcher.Models;

namespace Switcher.ViewModels
{
    public sealed class MainWindowViewModel : BaseViewModel
    {
        private Button _currentActiveButton;
        private List<Button> _switchButtons;
        private Config _appConfig;

        public MainWindowViewModel(IEnumerable<Button> switchButtons, string configPath)
        {
            _switchButtons = new List<Button>(switchButtons);

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

        public List<Button> SwitchButtons
        {
            get => _switchButtons;
            set
            {
                _switchButtons = value;
                OnPropertyChanged(nameof(SwitchButtons));
            }
        }

        public Button CurrentActiveButton
        {
            get => _currentActiveButton;
            set
            {
                _currentActiveButton = value;
                OnPropertyChanged(nameof(CurrentActiveButton));
            }
        }

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