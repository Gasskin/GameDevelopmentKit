using GameFramework;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public class PingAckHandler : PacketHandlerBase
    {
        public override int Id => SC_PingAck.MsgId;

        protected override void DoHandle(object sender, Packet packet)
        {
        }
    }
}

