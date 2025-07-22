using GameFramework;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public class PingAckHandler : PacketHandlerBase
    {
        public override int Id => GameHotMessageId.SC_PingAck;

        protected override void DoHandle(object sender, Packet packet)
        {
            var ack = (SC_PingAck)packet;
            HotEntry.ServerTime.Sync(ack.timeStamp);
        }
    }
}

