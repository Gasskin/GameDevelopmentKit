using System.Net;
using System.Net.Sockets;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public static class NetworkExtension
    {
        public static void CreateTcpChannel(this NetworkComponent net)
        {
            net.CreateNetworkChannel("TcpChannel", GameFramework.Network.ServiceType.Tcp, new NetworkChannelHelperHot());
            net.GetNetworkChannel("TcpChannel").Connect(IPAddress.Parse("127.0.0.1"), 12388);
        }
        
        public static void SendTcp(this NetworkComponent net, Packet packet)
        {
            var channel = net.GetNetworkChannel("TcpChannel");
            channel?.Send(packet);
        }
    }
}