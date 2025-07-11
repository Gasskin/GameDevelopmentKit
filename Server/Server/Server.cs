using System.Net;
using System.Net.Sockets;
using Game.Hot;
using ProtoBuf;

public class Server
{
    public static Server Instance { get; } = new();

    private const string IP = "127.0.0.1";
    private const int HOST = 12388;

    private Socket server;
    private Dictionary<Socket, ClientState> clients = new();
    private List<Socket> checkRead = new();

    public void Init()
    {
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        server.Bind(new IPEndPoint(IPAddress.Parse(IP), HOST));
        server.Listen(8);

        Console.WriteLine("[日志]启动成功");

        while (true)
        {
            checkRead.Clear();
            checkRead.Add(server);

            // 只添加连接有效的客户端
            foreach (var state in clients.Values.ToList())
            {
                if (state.socket != null && state.socket.Connected)
                    checkRead.Add(state.socket);
            }

            Socket.Select(checkRead, null, null, 1000);

            foreach (var socket in checkRead)
            {
                if (socket == server)
                {
                    AcceptClient();
                }
                else
                {
                    ReadClient(socket);
                }
            }
        }
    }

    private void AcceptClient()
    {
        var client = server.Accept();
        var state = new ClientState
        {
            socket = client
        };
        clients[client] = state;

        Console.WriteLine($"[日志]客户端登录{client.RemoteEndPoint}");
    }

    public bool ReadClient(Socket client)
    {
        if (!clients.TryGetValue(client, out var state))
            return false;

        if (client == null || !client.Connected)
        {
            DisconnectClient(client);
            return false;
        }

        try
        {
            int count = client.Receive(state.buff);
            if (count == 0)
            {
                DisconnectClient(client);
                return false;
            }

            state.receiveStream.Position = state.receiveStream.Length;
            state.receiveStream.Write(state.buff, 0, count);
            state.receiveStream.Position = 0;

            while (true)
            {
                if (state.receiveStream.Length - state.receiveStream.Position < 8)
                    break;

                byte[] headBuf = new byte[8];
                state.receiveStream.Read(headBuf, 0, 8);
                int bodyLength = BitConverter.ToInt32(headBuf, 0);
                int msgId = BitConverter.ToInt32(headBuf, 4);

                if (bodyLength <= 0 || bodyLength > 10_000_000)
                {
                    Console.WriteLine($"[日志]非法数据 Id={msgId}, Length={bodyLength}, From={client.RemoteEndPoint}");
                    DisconnectClient(client);
                    return false;
                }

                if (state.receiveStream.Length - state.receiveStream.Position < bodyLength)
                {
                    state.receiveStream.Position -= 8;
                    break;
                }

                byte[] bodyBuf = new byte[bodyLength];
                state.receiveStream.Read(bodyBuf, 0, bodyLength);

                HandleMessage(client, msgId, bodyBuf);
            }

            if (state.receiveStream.Position == state.receiveStream.Length)
            {
                state.receiveStream.SetLength(0);
            }
            else
            {
                byte[] remain = state.receiveStream.ToArray()[(int)state.receiveStream.Position..];
                state.receiveStream.SetLength(0);
                state.receiveStream.Write(remain, 0, remain.Length);
            }

            return true;
        }
        catch (ObjectDisposedException)
        {
            Console.WriteLine("[日志]已释放连接 读取中止");
            DisconnectClient(client);
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[日志]异常 {ex.Message}");
            DisconnectClient(client);
            return false;
        }
    }

    private void HandleMessage(Socket client, int msgId, byte[] bodyBuf)
    {
        Console.WriteLine($"[日志]接收 Id={msgId}, From={client.RemoteEndPoint}");
        using var ms = new MemoryStream(bodyBuf);

        try
        {
            switch (msgId)
            {
                case 30003:
                    var req = Serializer.Deserialize<CS_JoinRoomReq>(ms);
                    Room.Instance.JoinRoomReq(client, req);
                    break;

                default:
                    Console.WriteLine($"[日志]未知消息 Id={msgId}");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[日志]反序列化异常 Id={msgId}, 异常={ex.Message}");
        }
    }

    public void Send(Socket client, SCPacketBase packet)
    {
        if (client == null)
        {
            Console.WriteLine("[日志]发送失败 Socket为空");
            return;
        }

        // 不再检查 client.Connected

        byte[] body;
        using (var ms = new MemoryStream())
        {
            Serializer.Serialize(ms, packet);
            body = ms.ToArray();
        }

        int bodyLength = body.Length;
        int messageType = packet.Id;

        byte[] header = new byte[8];
        Array.Copy(BitConverter.GetBytes(bodyLength), 0, header, 0, 4);
        Array.Copy(BitConverter.GetBytes(messageType), 0, header, 4, 4);

        try
        {
            client.Send(header);
            client.Send(body);
            Console.WriteLine($"[日志]发送 Id={messageType}, Type={packet.GetType().Name}, To={client.RemoteEndPoint}");
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"[日志]发送失败 Socket错误: {ex.Message}");
            DisconnectClient(client);
        }
        catch (ObjectDisposedException)
        {
            Console.WriteLine("[日志]发送失败 客户端已关闭");
            DisconnectClient(client);
        }
    }


    private void DisconnectClient(Socket client)
    {
        if (clients.TryGetValue(client, out var state))
        {
            Console.WriteLine($"[日志]客户端断开 {client.RemoteEndPoint}");

            Room.Instance.OnDisconnect(client); // ✅ 清理房间玩家

            try
            {
                client.Shutdown(SocketShutdown.Both);
            }
            catch
            {
                // 忽略已关闭的 socket
            }

            client.Close();
            clients.Remove(client);
        }
    }

}
