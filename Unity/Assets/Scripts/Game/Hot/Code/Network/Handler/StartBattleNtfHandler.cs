using GameFramework.Network;

namespace Game.Hot
{
    public class StartBattleNtfHandler: PacketHandlerBase
    {
        public override int Id => GameHotMessageId.SC_StartBattleNtf;
        protected override void DoHandle(object sender, Packet packet)
        {
            
        }
    }
}