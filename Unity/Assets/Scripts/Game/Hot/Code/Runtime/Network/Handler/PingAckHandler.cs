using GameFramework;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public class PingAckHandler : PacketHandlerBase
    {
        public override int Id => 30002;

        protected override void DoHandle(object sender, Packet packet)
        {
        }
    }
}

