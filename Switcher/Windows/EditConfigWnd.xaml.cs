using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Switcher.Models;

namespace Switcher.Windows
{
    public partial class EditConfigWnd : Window
    {
        public Config Config { get; set; }
        
        public EditConfigWnd(Config currentConfig)
        {
            Config = currentConfig;
            this.DataContext = Config;
            InitializeComponent();
        }

        private void SaveConfig_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}