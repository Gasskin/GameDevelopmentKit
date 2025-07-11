using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        new Server.Server().Init();
        // Server.Instance.Init();
        // Battle.Instance.Init();
    }
}
