using GameFramework;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public class StartBattleNtfHandler : PacketHandlerBase
    {
        public override int Id => SC_StartBattleNtf.MsgId;
        
        protected override void DoHandle(object sender, Packet packet)
        {
        }
    }
}