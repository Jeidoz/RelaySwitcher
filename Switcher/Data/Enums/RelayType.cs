using System.ComponentModel;

namespace Switcher.Data.Enums
{
    public enum RelayType : byte
    {
        [Description("4 Channels relay")]
        FourChannels,
        [Description("6 Channels relay")]
        SixChannels,
        [Description("8 channels relay")]
        EightChannels
    }
}
