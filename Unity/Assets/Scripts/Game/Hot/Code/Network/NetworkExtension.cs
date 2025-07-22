using System.Net.Sockets;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public static class NetworkExtension
    {
        public static void SendTcp(this NetworkComponent net, Packet packet)
        {
            var channel = net.GetNetworkChannel("ChannelTcp");
            channel?.Send(packet);
        }
    }
}