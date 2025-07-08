using GameFramework.Network;
using Game.Hot;

namespace Game.Hot
{
    public class SCHeartBeatTestHandler : PacketHandlerBase
    {
        public override int Id => 30002;

        public override void Handle(object sender, Packet packet)
        {
            var data = (SCHeartBeatTest)packet;
        }
    }
}