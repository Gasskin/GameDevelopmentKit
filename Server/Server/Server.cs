using System.Net;
using System.Net.Sockets;
using System.Text;
using Game.Hot;
using ProtoBuf;

namespace Server
{
    internal class Server
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
        private List<Socket> checkRead = new List<Socket>();

        public void Init()
        {
            //Socket
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Bind
            var ipAdr = IPAddress.Parse(IP);
            var ipEp = new IPEndPoint(ipAdr, HOST);
            server.Bind(ipEp);
            //Listen
            server.Listen(0);
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
                        ReadClientfd(socket);
                    }
                }
            }
        }

        //读取Listenfd
        public void AcceptClient(Socket server)
        {
            var clientfd = server.Accept();
            var state = new ClientState();
            state.socket = clientfd;
            clients.Add(clientfd, state);

            Console.WriteLine($"[客户端登录]{clientfd.RemoteEndPoint}");
        }

        public bool ReadClientfd(Socket clientfd)
        {
            ClientState state = clients[clientfd];

            try
            {
                int count = clientfd.Receive(state.buff);
                if (count == 0)
                {
                    Console.WriteLine($"[客户端关闭]{clientfd.RemoteEndPoint}");
                    clientfd.Close();
                    clients.Remove(clientfd);
                    return false;
                }

                // 将接收到的数据写入 MemoryStream
                state.stream.Position = state.stream.Length;
                state.stream.Write(state.buff, 0, count);
                state.stream.Position = 0;

                // 循环处理多个完整包（8字节头 + body）
                while (true)
                {
                    // 检查是否足够读取头部
                    if (state.stream.Length - state.stream.Position < 8)
                        break;

                    byte[] headBuf = new byte[8];
                    state.stream.Read(headBuf, 0, 8);

                    int bodyLength = BitConverter.ToInt32(headBuf, 0);
                    int msgId = BitConverter.ToInt32(headBuf, 4);

                    // 校验 body 长度合法性
                    if (bodyLength <= 0 || bodyLength > 10_000_000)
                    {
                        Console.WriteLine($"[非法数据长度] Id={msgId}, Length={bodyLength}");
                        clientfd.Close();
                        clients.Remove(clientfd);
                        return false;
                    }

                    // 检查是否足够读取 body
                    if (state.stream.Length - state.stream.Position < bodyLength)
                    {
                        // 回退头部位置
                        state.stream.Position -= 8;
                        break;
                    }

                    byte[] bodyBuf = new byte[bodyLength];
                    state.stream.Read(bodyBuf, 0, bodyLength);
                    string msg = Encoding.UTF8.GetString(bodyBuf);


                    using var ms = new MemoryStream(bodyBuf);
                    switch (msgId)
                    {
                        case 30003:
                            var body = Serializer.Deserialize<CS_JoinRoomReq>(ms);
                            Console.WriteLine($"[接收消息] Id={msgId}, Body={body}");
                            break;

                        default:
                            break;
                    }
                }

                // 清理已读数据
                if (state.stream.Position == state.stream.Length)
                {
                    state.stream.SetLength(0);
                }
                else
                {
                    byte[] remain = state.stream.ToArray()[(int)state.stream.Position..];
                    state.stream.SetLength(0);
                    state.stream.Write(remain, 0, remain.Length);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[异常]{ex.Message}");
                clientfd.Close();
                clients.Remove(clientfd);
                return false;
            }
        }
    }
}