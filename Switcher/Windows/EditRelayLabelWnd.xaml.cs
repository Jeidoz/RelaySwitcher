using Switcher.Models;
using System.Windows;

namespace Switcher.Windows
{
    /// <summary>
    /// Interaction logic for EditRelayLabelWnd.xaml
    /// </summary>
    public partial class EditRelayLabelWnd : Window
    {
        private readonly RelayLabel _viewModel;

        public string NewLabel => _viewModel.Label;

        public EditRelayLabelWnd(RelayLabel relayLabelInfo)
        {
            _viewModel = new RelayLabel
            {
                RelayChannel = relayLabelInfo.RelayChannel,
                Label = relayLabelInfo.Label
            };
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void SaveLabel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
