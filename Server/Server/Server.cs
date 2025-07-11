using System.Net;
using System.Net.Sockets;
using Game.Hot;
using ProtoBuf;

public class Server
{
    public static Server Instance { get; } = new();


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
                    ReadClient(socket);
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
    }

    //读取Clientfd
    public bool ReadClient(Socket client)
    {
        ClientState state = clients[client];

        try
        {
            int count = client.Receive(state.buff);
            if (count == 0)
            {
                Console.WriteLine($"[客户端关闭]{client.RemoteEndPoint}");
                client.Close();
                clients.Remove(client);
                return false;
            }

            // 把收到的数据追加进 MemoryStream
            state.receiveStream.Position = state.receiveStream.Length;
            state.receiveStream.Write(state.buff, 0, count);
            state.receiveStream.Position = 0;

            // 处理所有完整包
            while (true)
            {
                // 检查是否能读出头（8字节）
                if (state.receiveStream.Length - state.receiveStream.Position < 8)
                {
                    break; // 等待更多数据
                }

                // 读取头
                byte[] headBuf = new byte[8];
                state.receiveStream.Read(headBuf, 0, 8);
                int bodyLength = BitConverter.ToInt32(headBuf, 0);
                int msgId = BitConverter.ToInt32(headBuf, 4);

                // 检查是否收到了完整 body
                if (state.receiveStream.Length - state.receiveStream.Position < bodyLength)
                {
                    // 回退 8 字节（头）等待下次完整 body
                    state.receiveStream.Position -= 8;
                    break;
                }

                // 读取 body
                byte[] bodyBuf = new byte[bodyLength];
                state.receiveStream.Read(bodyBuf, 0, bodyLength);

                // 处理协议
                HandleMessage(client, msgId, bodyBuf);
            }

            // 清理已读数据
            if (state.receiveStream.Position == state.receiveStream.Length)
            {
                state.receiveStream.SetLength(0); // 全部读完了
            }
            else
            {
                // 剩下没读完的内容前移
                byte[] remain = state.receiveStream.ToArray()[(int)state.receiveStream.Position..];
                state.receiveStream.SetLength(0);
                state.receiveStream.Write(remain, 0, remain.Length);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[异常] {ex.Message}");
            client.Close();
            clients.Remove(client);
            return false;
        }
    }

    private void HandleMessage(Socket client, int msgId, byte[] bodyBuf)
    {
        using var ms = new MemoryStream(bodyBuf);
        switch (msgId)
        {
            case 30003:
                Room.Instance.JoinRoomReq(client, Serializer.Deserialize<CS_JoinRoomReq>(ms));
                break;

            default:
                Console.WriteLine($"[未知消息] Id={msgId}");
                break;
        }
    }


    /// <summary>
    /// 发送一个 SCPacketBase 协议对象，封装消息头并通过 TCP 发送。
    /// </summary>
    public void Send(Socket client, SCPacketBase packet)
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

            Console.WriteLine($"[发送] Id={messageType}, Type={packet.GetType().Name}, To={client.RemoteEndPoint}");
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"[发送失败] Socket错误: {ex.Message}");
        }
    }
}