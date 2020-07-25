using Switcher.ViewModels;
using System.Windows;

namespace Switcher.Models
{
    public sealed class RelayButton : BaseViewModel
    {
        private Style _style;
        private bool _isEnabled;
        private RelayLabel _relayLabel;


        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }
        public Style Style
        {
            get => _style;
            set
            {
                _style = value;
                OnPropertyChanged(nameof(Style));
            }
        }

        public RelayLabel RelayLabel
        {
            get => _relayLabel;
            set
            {
                _relayLabel = value;
                OnPropertyChanged(nameof(Style));
            }
        }
    }
}
