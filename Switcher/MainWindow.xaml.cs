using Switcher.Models;
using Switcher.ViewModels;
using Switcher.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Switcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainWindowViewModel _viewModel;
        private readonly string _configFilePath;
        private readonly Style _activatedButtonStyle;
        private readonly Style _deactivatedButtonStyle;

        private ChannelManager _channelManager;

        public MainWindow()
        {
            _configFilePath = Path.Combine(Directory.GetCurrentDirectory(), Config.FileName);
            InitializeComponent();

            var buttons = new List<Button>
            {
                Channel1,
                Channel2,
                Channel3,
                Channel4
            };
            _viewModel = new MainWindowViewModel(buttons, _configFilePath);
            DataContext = _viewModel;
            _activatedButtonStyle = FindResource("OnSwitch") as Style;
            _deactivatedButtonStyle = FindResource("OffSwitch") as Style;
            try
            {
                _channelManager = new ChannelManager(_viewModel.AppConfig);
                SetUpActiveButtons();
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show(
                    "Received unknown response. Try to change port/password in the config.json",
                    "Unknown response",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                Application.Current.Shutdown();
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Can not establish connection to remote host. Try to change ip/port/password in the config.json.",
                    "Can not connect to remote host",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                Application.Current.Shutdown();
            }
        }

        private void SetUpActiveButtons()
        {
            var channelsStatus = _channelManager.GetRelayChannelsStatus();
            for (int i = 0; i < 4; ++i)
            {
                _viewModel.SwitchButtons[i].Style =
                    channelsStatus[i] ? _activatedButtonStyle : _deactivatedButtonStyle;
            }
        }

        private async void ProcessButtonClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button pressedButton))
            {
                return;
            }

            pressedButton.Style = _activatedButtonStyle;
            Enum.TryParse(pressedButton.Tag.ToString(), out Channels selectedChannel);
            await _channelManager.SendRelayChannelsSetCommand(selectedChannel);
            foreach (var button in _viewModel.SwitchButtons.Where(btn => btn.Name != pressedButton.Name))
            {
                button.Style = _deactivatedButtonStyle;
            }
        }

        private void MenuItem_ThirdPartyLibs_OnClick(object sender, RoutedEventArgs e)
        {
            var wnd = new ThirdPartyLibrariesWnd();
            wnd.Show();
        }

        private void MenuItem_AboutApp_OnClick(object sender, RoutedEventArgs e)
        {
            var wnd = new AboutAppWnd();
            wnd.Show();
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

            try
            {
                _channelManager = new ChannelManager(_viewModel.AppConfig);
                SetUpActiveButtons();
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show(
                    "Received unknown response. Try to change port/password in the app config.",
                    "Unknown response",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Can not establish connection to remote host. Try to change ip/port/password in the app config.",
                    "Can not connect to remote host",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
    }
}