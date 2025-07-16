using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public class JoinRoomNtfHandler : PacketHandlerBase
    {
        public override int Id => 30005;

        public override void Handle(object sender, Packet packet)
        {
            var msg = (SC_JoinRoomNtf)packet;
            HotEntry.Model.Room.AddRoomPlayer(msg.newAccountId, msg.roomPlayers);
        }
    }
}