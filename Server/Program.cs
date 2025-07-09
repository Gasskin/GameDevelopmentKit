using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        var server = new Server();
        server.Init();
    }
}
