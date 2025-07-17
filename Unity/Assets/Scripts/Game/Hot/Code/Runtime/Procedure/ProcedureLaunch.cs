using System.Net;
using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<Game.Hot.ProcedureComponent>;

namespace Game.Hot
{
    public sealed class ProcedureLaunch : ProcedureBase
    {
        private VarInt32 _battleStage;
        
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            _battleStage = ReferencePool.Acquire<VarInt32>();
            _battleStage.Value = (int)BattleStage.None;
            GameEntry.DataNode.SetData(HotConstant.DataNode.BATTLE_STAGE, _battleStage);
            
            GameEntry.UI.OpenUIForm(UIFormId.JoinRoomForm);
        }

        protected override void OnUpdate(IFsm<ProcedureComponent> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (_battleStage.Value == (int)BattleStage.LoadGameAsset)
            {
                ChangeState<ProcedureLoadGameAsset>(procedureOwner);
            }
        }
    }
}