using GameFramework.Network;

namespace Game.Hot
{
    public class LeaveRoomNtf : PacketHandlerBase
    {
        public override int Id => 30006;

        protected override void DoHandle(object sender, Packet packet)
        {
            var msg = (SC_LeaveRoomNtf)packet;
            HotEntry.Model.Room.RemoveRoomPlayer(msg.leaveAccountId);
        }
    }
}