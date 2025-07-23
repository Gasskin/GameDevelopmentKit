using GameFramework.Network;

namespace Game.Hot
{
    public class BeginBattleNtfHandler : PacketHandlerBase
    {
        public override int Id => GameHotMessageId.SC_BeginBattleNtf;
        
        protected override void DoHandle(object sender, Packet packet)
        {
            HotEntry.Model.RoomBattle.SC_BeginBattleNtf((SC_BeginBattleNtf)packet);
        }
    }
}