namespace Moongate.Core.Network.Extensions;

public static class PacketExtensions
{
    public static string ToPacketString(this byte opCode)
    {
        return "0x" + opCode.ToString("X2");
    }

}
