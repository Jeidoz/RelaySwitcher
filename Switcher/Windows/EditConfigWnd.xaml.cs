﻿using Switcher.Models;
using System.Windows;

namespace Switcher.Windows
{
    public partial class EditConfigWnd : Window
    {
        // ReSharper disable once MemberCanBePrivate.Global
        // Required for XAML Binding
        public Config Config { get; set; }

        public EditConfigWnd(Config currentConfig)
        {
            Config = new Config(currentConfig);
            DataContext = Config;
            InitializeComponent();
        }

        private void SaveConfig_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}