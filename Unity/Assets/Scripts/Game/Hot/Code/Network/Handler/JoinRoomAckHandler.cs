using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public class JoinRoomAckHandler : PacketHandlerBase
    {
        public override int Id => GameHotMessageId.SC_JoinRoomAck;

        protected override bool HasAck => true;

        protected override void DoHandle(object sender, Packet packet)
        {
            var msg = (SC_JoinRoomAck)packet;
            HotEntry.Model.Room.SetRoomPlayers(msg.myAccountId, msg.roomPlayers);
            HotEntry.Model.Room.ChangeRoomStage(EBattleStage.InRoom);
        }
    }
}