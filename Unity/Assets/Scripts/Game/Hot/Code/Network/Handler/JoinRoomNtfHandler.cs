using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public class JoinRoomNtfHandler : PacketHandlerBase
    {
        public override int Id => GameHotMessageId.SC_JoinRoomNtf;

        protected override void DoHandle(object sender, Packet packet)
        {
            var msg = (SC_JoinRoomNtf)packet;
            HotEntry.Model.Room.AddRoomPlayer(msg.newAccountId, msg.roomPlayers);
        }
    }
}