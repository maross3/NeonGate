namespace NeonGateWeb.Global;
public enum RequestType
{
    Get,
    Post
}

// todo, this is designed for the 4 bytes in byte array's header
// this is implementation specific. 
[Flags]
public enum FirstUdpMessage : byte
{
    Bit0 = 1 << 0,
    Bit1 = 1 << 1,
    Bit2 = 1 << 2,
    Bit3 = 1 << 3,
    Bit4 = 1 << 4,
    Bit5 = 1 << 5,
    Bit6 = 1 << 6,
    Bit7 = 1 << 7
}

[Flags]
public enum SecondUdpMessage : byte
{
    Bit0 = 1 << 0,
    Bit1 = 1 << 1,
    Bit2 = 1 << 2,
    Bit3 = 1 << 3,
    Bit4 = 1 << 4,
    Bit5 = 1 << 5,
    Bit6 = 1 << 6,
    Bit7 = 1 << 7
}

[Flags]
public enum ThirdUdpMessage : byte
{
    Bit0 = 1 << 0,
    Bit1 = 1 << 1,
    Bit2 = 1 << 2,
    Bit3 = 1 << 3,
    Bit4 = 1 << 4,
    Bit5 = 1 << 5,
    Bit6 = 1 << 6,
    Bit7 = 1 << 7
}

[Flags]
public enum FourthUdpMessage
{
    None = 0,
    Start = 1,
    Stop = 2,
    Restart = 4,
    Status = 8,
    Exit = 16,
    All = Start | Stop | Restart | Status | Exit
}