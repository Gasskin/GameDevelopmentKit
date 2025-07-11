using System.Net.Sockets;

internal class ClientState
{
    public Socket socket;
    public byte[] buff = new byte[1024];
    public MemoryStream stream = new MemoryStream();
}
