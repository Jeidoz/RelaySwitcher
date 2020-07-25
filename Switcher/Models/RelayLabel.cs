using Switcher.Data.Enums;
using Switcher.ViewModels;

namespace Switcher.Models
{
    public sealed class RelayLabel : BaseViewModel
    {
        private Channels _relayChannel;
        private string _label;

        public Channels RelayChannel
        {
            get => _relayChannel;
            set
            {
                _relayChannel = value;
                OnPropertyChanged(nameof(RelayChannel));
            }
        }

        public string Label
        {
            get => _label;
            set
            {
                _label = value;
                OnPropertyChanged(nameof(Label));
            }
        }

        public RelayLabel()
        {

        }

        public RelayLabel(Channels channel, string label)
        {
            _relayChannel = channel;
            _label = label;
        }
    }
}
