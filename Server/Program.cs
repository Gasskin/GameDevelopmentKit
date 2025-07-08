using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        // 监听地址和端口（本地回环地址）
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        int port = 9000;

        // 创建 socket
        Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // 绑定 IP 和端口
        listener.Bind(new IPEndPoint(ipAddress, port));

        // 开始监听（最多同时挂起 10 个连接请求）
        listener.Listen(10);

        Console.WriteLine($"服务器启动，监听 {ipAddress}:{port} ...");

        while (true)
        {
            Console.WriteLine("等待客户端连接...");
            Socket handler = listener.Accept();  // 阻塞直到有连接

            Console.WriteLine("客户端已连接！");

            byte[] buffer = new byte[1024];
            int bytesRead = handler.Receive(buffer);

            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine("收到消息：" + receivedMessage);

            // 可选：关闭连接
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
    }
}
