using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        Server.Instance.Init();
        Battle.Instance.Init();
    }
}
