using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<Game.Hot.ProcedureComponent>;

namespace Game.Hot
{
    public class ProcedureLoadGameAsset : ProcedureBase
    {
        private int _loadingForm;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.UI.OpenUIForm(UIFormId.LoadingForm);
        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            // if (_battleStage.Value == (int)EBattleStage.ReadyForGame)
            // {
            //     _battleStage.Value = 0;
            //     var msg = ReferencePool.Acquire<CS_ReadyForGameNtf>();
            //     msg.accountId = HotEntry.Model.Room.MyAccount;
            //     GameEntry.Network.SendTcp(msg);
            // }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }
    }
}