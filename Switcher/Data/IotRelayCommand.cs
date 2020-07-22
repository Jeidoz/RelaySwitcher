using System;

namespace Switcher.Data
{
    public sealed class IotRelayCommand
    {
        public const byte StatusResultByte = 4;
        public const byte SetRelay = 0xFF;
        public const byte ResultXorKey = 0xAA;
        public const ushort MinPasswordValue = 0;
        public const ushort MaxPasswordValue = 9999;
        public const byte NoPassword = 0;
        public const byte AllChannelsMask = 0b1111;
        public const byte StatusCommand = 0;
        public const byte WriteCommand = 1;
        public const byte PCToDeviceResult = 0 ^ ResultXorKey;

        private byte[] _lsbPassword;

        public byte Session { get; set; }

        public ushort Password
        {
            set
            {
                if (value > MaxPasswordValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(Password), $"Out of password range ({MinPasswordValue}~{MaxPasswordValue})");
                }

                // 1234 to { 34, 12 }
                int[] bytes =
                {
                    value % 100,
                    (value - value % 100) / 100
                };

                // { 34, 12 } to { 0x34, 0x12 }
                _lsbPassword = new[]
                {
                    (byte) (bytes[0] / 10 * 16 + bytes[0] % 10),
                    (byte) (bytes[1] / 10 * 16 + bytes[1] % 10)
                };
            }
        }

        public IotRelayCommand(int password = NoPassword)
        {
            Session = default;
            Password = (ushort)password;
        }

        public byte[] GetRelayChannelsSetCommand(Channels channel)
        {
            byte selectedChannel = channel switch
            {
                Channels.None => 0b0000,
                Channels.First => 0b0001,
                Channels.Second => 0b0010,
                Channels.Third => 0b0100,
                Channels.Fourth => 0b1000,
                _ => throw new ArgumentOutOfRangeException(nameof(channel), "Out of relay channels (range is #1-#4)")
            };
            return new[]
            {
                SetRelay, PCToDeviceResult, default, WriteCommand,
                _lsbPassword[0], _lsbPassword[1],
                AllChannelsMask, selectedChannel
            };
        }

        public byte[] GetRelayChannelsStatusCommand()
        {
            return new[]
            {
                SetRelay, PCToDeviceResult, default, StatusCommand,
                _lsbPassword[0], _lsbPassword[1]
            };
        }
    }
}