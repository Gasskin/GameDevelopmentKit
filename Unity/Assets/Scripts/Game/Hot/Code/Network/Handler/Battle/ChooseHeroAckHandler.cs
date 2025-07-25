using GameFramework.Network;

namespace Game.Hot
{
    public class ChooseHeroAckHandler: PacketHandlerBase
    {
        public override int Id => GameHotMessageId.SC_ChooseHeroAck;
        protected override int AwaitReqId => GameHotMessageId.CS_ChooseHeroReq;

        protected override void DoHandle(object sender, Packet packet)
        {
            
        }
    }
}