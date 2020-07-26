using Switcher.ViewModels;
using System.Windows;

namespace Switcher.Models
{
    public sealed class RelayButton : BaseViewModel
    {
        private bool _isVisible;
        private Style _style;
        private RelayLabel _relayLabel;

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
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
