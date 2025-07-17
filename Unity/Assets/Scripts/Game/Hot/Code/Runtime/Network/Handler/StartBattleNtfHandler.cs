using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public class StartBattleNtfHandler : PacketHandlerBase
    {
        public override int Id => 30008;
        
        protected override void DoHandle(object sender, Packet packet)
        {
            Log.Info("开始战斗！");
        }
    }
}