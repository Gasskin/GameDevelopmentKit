using GameFramework;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public class PingAckHandler : PacketHandlerBase
    {
        public override int Id => 30002;

        public override void Handle(object sender, Packet packet)
        {
            var msg = (SC_PingAck)packet;
            Log.Debug(Utility.FormatTime(msg.timeStamp));
        }
    }
}

