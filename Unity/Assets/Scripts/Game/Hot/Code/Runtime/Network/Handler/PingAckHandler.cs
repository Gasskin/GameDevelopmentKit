using GameFramework.Network;

namespace Game.Hot
{
    public class PingAckHandler : PacketHandlerBase
    {
        public override int Id => 30002;

        public override void Handle(object sender, Packet packet)
        {
        }
    }
}

