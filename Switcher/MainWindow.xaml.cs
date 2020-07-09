using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Switcher.Models;
using Switcher.ViewModels;
using Switcher.Windows;

namespace Switcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainWindowViewModel _viewModel;
        
        private readonly string _configFilePath;
        private ChannelManager _channelManager;
        private readonly Style _activatedButtonStyle;
        private readonly Style _deactivatedButtonStyle;
        
        public MainWindow()
        {
            _configFilePath = Path.Combine(Directory.GetCurrentDirectory(), Config.FileName);
            _channelManager = new ChannelManager();
            InitializeComponent();
            
            var buttons = new List<Button>
            {
                Channel1,
                Channel2,
                Channel3,
                Channel4
            };
            _viewModel = new MainWindowViewModel(buttons, _configFilePath);
            this.DataContext = _viewModel;
            _activatedButtonStyle = this.FindResource("OnSwitch") as Style;
            _deactivatedButtonStyle = this.FindResource("OffSwitch") as Style;
        }

        private void ProcessButtonClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button pressedButton))
            {
                return;
            }

            pressedButton.Style = _activatedButtonStyle;
            foreach (var button in _viewModel.SwitchButtons.Where(btn => btn.Name != pressedButton.Name))
            {
                button.Style = _deactivatedButtonStyle;
            }
        }

        private void MenuItem_ThirdPartyLibs_OnClick(object sender, RoutedEventArgs e)
        {
            var wnd = new ThirdPartyLibrariesWnd();
            wnd.ShowDialog();
        }

        private void MenuItem_OpenConfigFile_OnClick(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(_configFilePath))
            {
                _viewModel.AppConfig.SaveToFile(_configFilePath);
                MessageBox.Show("Could not find configuration file. In the app folder has been created config.json file with current app configuration",
                    "Missing requested config file",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }

            Process.Start(new ProcessStartInfo(_configFilePath));
        }

        private void MenuItem_EditConfig_OnClick(object sender, RoutedEventArgs e)
        {
            var wnd = new EditConfigWnd(_viewModel.AppConfig);
            if (wnd.ShowDialog() != true)
            {
                return;
            }

            _viewModel.AppConfig = wnd.Config;
            _viewModel.AppConfig.SaveToFile(_configFilePath);
        }
    }
}