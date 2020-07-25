using Switcher.ViewModels;
using System.Windows;

namespace Switcher.Models
{
    public sealed class RelayButton : BaseViewModel
    {
        private Style _style;
        private RelayLabel _relayLabel;

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
