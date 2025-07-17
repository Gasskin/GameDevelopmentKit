using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public class JoinRoomAckHandler : PacketHandlerBase
    {
        public override int Id => 30004;

        public override int FromReqId => 30003;

        protected override void DoHandle(object sender, Packet packet)
        {
            var msg = (SC_JoinRoomAck)packet;
            HotEntry.Model.Room.SetRoomPlayers(msg.myAccountId, msg.roomPlayers);
        }
    }
}