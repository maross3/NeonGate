namespace NeonGateWeb.Global;

public static class DatagramConstants
{
    public const int MAX_PACKET_LEN = short.MaxValue;
    public const int HOST_PORT = 8080;
    public const int CONN_PORT = 8081;
    public const int MAX_CONNECTIONS = 10;
    public const int MAX_PACKET_PROCESS_PER_FRAME = int.MaxValue;
}
