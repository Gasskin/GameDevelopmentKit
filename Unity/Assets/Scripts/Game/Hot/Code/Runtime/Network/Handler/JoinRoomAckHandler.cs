using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public class JoinRoomAckHandler : PacketHandlerBase
    {
        public override int Id => 30004;

        public override void Handle(object sender, Packet packet)
        {
            var msg = (SC_JoinRoomAck)packet;
            HotEntry.Model.Room.SetRoomPlayers(msg.roomPlayers);
        }
    }
}