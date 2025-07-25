using GameFramework.Event;
using GameFramework.UI;
using UnityGameFramework.Runtime;
using OpenUIFormSuccessEventArgs = UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs;
using ProcedureOwner = GameFramework.Fsm.IFsm<Game.Hot.ProcedureComponent>;

namespace Game.Hot
{
    public class ProcedureInBattle : ProcedureBase
    {
        private int _battleForm;
        private bool _isOpenBattleForm;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            _battleForm = GameEntry.UI.OpenUIForm(UIFormId.BattleForm) ?? 0;
            _isOpenBattleForm = false;
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
        }



        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (!_isOpenBattleForm)
                return;
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            if (_battleForm > 0)
                GameEntry.UI.CloseUIForm(_battleForm);
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
        }
        
        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            var ee = (OpenUIFormSuccessEventArgs)e;
            if (ee.UIForm.SerialId == _battleForm)
                _isOpenBattleForm = true;
        }
    }
}