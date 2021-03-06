﻿using Switcher.Data.Enums;
using Switcher.Models;
using Switcher.ViewModels;
using Switcher.Windows;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public static ICommand EditRelayLabelCommand = new RoutedCommand();


        public MainWindow()
        {
            _configFilePath = Path.Combine(Directory.GetCurrentDirectory(), Config.FileName);
            if (!File.Exists(_configFilePath))
            {
                MessageBox.Show("Could not find configuration file. In the app folder has been created config.json file with current app configuration",
                    "Missing requested config file",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
            _viewModel = new MainWindowViewModel(_configFilePath);
            DataContext = _viewModel;
            InitializeComponent();

            _activatedButtonStyle = FindResource("OnSwitch") as Style;
            _deactivatedButtonStyle = FindResource("OffSwitch") as Style;

            var buttons = new ObservableCollection<RelayButton>();
            for (int i = 0; i < 8; ++i)
            {
                buttons.Add(new RelayButton
                {
                    Style = _deactivatedButtonStyle,
                    RelayLabel = _viewModel.AppConfig.RelayLabels[i]
                });
            }

            _viewModel.SwitchButtons = buttons;
            SwitchButtonContainer.ItemsSource = _viewModel.SwitchButtons;

            {
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
        }

        private void SetUpActiveButtons()
        {
            var channelsStatus = _channelManager.GetRelayChannelsStatus();
            for (int i = 0; i < channelsStatus.Count; ++i)
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
            var context = (RelayButton)pressedButton.DataContext;
            Channels selectedChannel = context.RelayLabel.RelayChannel;
            await _channelManager.SendRelayChannelsSetCommand(selectedChannel);
            var buttonsToDeactivate = _viewModel.SwitchButtons.Where(btn =>
                btn.RelayLabel.RelayChannel != selectedChannel && btn.Style == _activatedButtonStyle);
            foreach (var button in buttonsToDeactivate)
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

            if (_viewModel.AppConfig.Equals(wnd.Config))
            {
                return;
            }

            _viewModel.AppConfig = new Config(wnd.Config);
            _viewModel.AppConfig.SaveToFile(_configFilePath);
            if (_viewModel.AppConfig.IsSameSocketConfig(wnd.Config))
            {
                _channelManager.SetConfigWithSameSocket(wnd.Config);
            }
            else
            {
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

        private void ExecutedEditRelayButtonLabel(object sender, ExecutedRoutedEventArgs e)
        {
            var context = (RelayButton)((Button)e.OriginalSource).DataContext;
            var oldLabel = context.RelayLabel;
            var wnd = new EditRelayLabelWnd(oldLabel);
            if (wnd.ShowDialog() == true)
            {
                oldLabel.Label = wnd.NewLabel;
                _viewModel.AppConfig.SaveToFile(_configFilePath);
            }
        }

        private void CanExecuteEditRelayButtonLabel(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.OriginalSource is Button;
        }
    }
}