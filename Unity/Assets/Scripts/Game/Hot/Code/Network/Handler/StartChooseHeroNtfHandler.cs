using GameFramework.Network;

namespace Game.Hot
{
    public class StartChooseHeroNtfHandler : PacketHandlerBase
    {
        public override int Id => GameHotMessageId.SC_StartChooseHeroNtf;
        
        protected override void DoHandle(object sender, Packet packet)
        {
            HotEntry.Model.RoomBattle.SC_BeginBattleNtf((SC_StartChooseHeroNtf)packet);
        }
    }
}