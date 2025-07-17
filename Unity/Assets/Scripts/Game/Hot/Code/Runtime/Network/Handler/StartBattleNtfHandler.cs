using GameFramework;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public class StartBattleNtfHandler : PacketHandlerBase
    {
        public override int Id => 30008;
        
        protected override void DoHandle(object sender, Packet packet)
        {
            var battleStage = GameEntry.DataNode.GetData<VarInt32>(HotConstant.DataNode.BATTLE_STAGE);
            battleStage.Value = (int)BattleStage.LoadGameAsset;
        }
    }
}