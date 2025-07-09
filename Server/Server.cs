using System.Net;
using System.Net.Sockets;
using System.Text;
using Game.Hot;
using ProtoBuf;

internal class Server : IDisposable
{
    // IP地址
    private const string IP = "127.0.0.1";

    // 端口号
    private const int HOST = 12388;

    // 服务器Socket
    private Socket server;

    // 客户端Socket及状态信息
    private Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();

    // 多路复用
    List<Socket> checkRead = new List<Socket>();

    public void Init()
    {
        //Socket
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //Bind
        var ipAdr = IPAddress.Parse(IP);
        var ipEp = new IPEndPoint(ipAdr, HOST);
        server.Bind(ipEp);
        //Listen
        server.Listen(8);
        Console.WriteLine("[服务器]启动成功");

        //主循环
        while (true)
        {
            //填充checkRead列表
            checkRead.Clear();
            checkRead.Add(server);
            foreach (var state in clients.Values)
                checkRead.Add(state.socket);
            //select
            Socket.Select(checkRead, null, null, 1000);
            //检查可读对象
            foreach (var socket in checkRead)
            {
                if (socket == server)
                {
                    AcceptClient(socket);
                }
                else
                {
                    // ReadClientfd(socket);
                }
            }
        }
    }

    //读取Listenfd
    public void AcceptClient(Socket server)
    {
        var client = server.Accept();
        var state = new ClientState();
        state.socket = client;
        clients.Add(client, state);

        Console.WriteLine($"[客户端登录]{client.RemoteEndPoint}");

        // SendTestMessage(client);
        SendMsg(client, new SCHeartBeatTest()
        {
            A = [1, 2, 3, 4, 5]
        });
    }

    //读取Clientfd
    public bool ReadClientfd(Socket clientfd)
    {
        ClientState state = clients[clientfd];
        //接收
        int count = 0;
        try
        {
            count = clientfd.Receive(state.buff);
        }
        catch (SocketException ex)
        {
            clientfd.Close();
            clients.Remove(clientfd);
            return false;
        }
        //客户端关闭
        if (count == 0)
        {
            Console.WriteLine($"[客户端关闭]{clientfd.LocalEndPoint}");
            clientfd.Close();
            clients.Remove(clientfd);
            return false;
        }
        //广播
        string recvStr = Encoding.UTF8.GetString(state.buff, 0, count);
        Console.WriteLine($"[接受客户端消息]{recvStr}");
        byte[] sendBytes = Encoding.UTF8.GetBytes(recvStr);
        foreach (ClientState cs in clients.Values)
        {
            cs.socket.Send(sendBytes);
        }
        return true;
    }

    public void Dispose()
    {
        if (server != null)
        {
            Console.WriteLine("[服务器]关闭");
            server.Dispose();
        }
    }

    // void SendTestMessage(Socket client)
    // {
    //     var msg = new SCHeartBeatTest
    //     {
    //         A = new List<int> { 121, 122, 123, 124, 125 },
    //     };
    //
    //     // 使用 protobuf-net 序列化
    //     byte[] body;
    //     using (var ms = new MemoryStream())
    //     {
    //         Serializer.Serialize(ms, msg);
    //         body = ms.ToArray();
    //     }
    //     Console.WriteLine("发送字节：" + BitConverter.ToString(body));
    //
    //     int bodyLength = body.Length;
    //     int messageType = msg.Id; // 👈 你客户端必须注册过的协议ID
    //
    //     byte[] header = new byte[8];
    //     Array.Copy(BitConverter.GetBytes(bodyLength), 0, header, 0, 4);
    //     Array.Copy(BitConverter.GetBytes(messageType), 0, header, 4, 4);
    //
    //     client.Send(header); // 👈 只发头，不带消息体
    //     client.Send(body);
    // }


    /// <summary>
    /// 发送一个 SCPacketBase 协议对象，封装消息头并通过 TCP 发送。
    /// </summary>
    public static void SendMsg(Socket client, SCPacketBase packet)
    {
        // 1. 序列化 proto 消息体
        byte[] body;
        using (var ms = new MemoryStream())
        {
            Serializer.Serialize(ms, packet);
            body = ms.ToArray();
        }

        int bodyLength = body.Length;
        int messageType = packet.Id;

        // 2. 构造 8 字节消息头：[4字节长度][4字节协议Id]
        byte[] header = new byte[8];
        Array.Copy(BitConverter.GetBytes(bodyLength), 0, header, 0, 4);
        Array.Copy(BitConverter.GetBytes(messageType), 0, header, 4, 4);

        try
        {
            // 3. 发送头 + 体
            client.Send(header);
            client.Send(body);

            Console.WriteLine($"[发送] Id={messageType}, Len={bodyLength}, Type={packet.GetType().Name}");
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"[发送失败] Socket错误: {ex.Message}");
        }
    }
}