using GameFramework.Fsm;

namespace Game.Hot
{
    public class ProcedureJoinRoom:ProcedureBase
    {
        private int? _joinRoomForm;

        protected override void OnEnter(IFsm<ProcedureComponent> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            _joinRoomForm = GameEntry.UI.OpenUIForm(UIFormId.JoinRoomForm);
            HotEntry.Model.Room.ChangeBattleStage(EBattleStage.None);
        }

        protected override void OnUpdate(IFsm<ProcedureComponent> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (HotEntry.Model.Room.BattleStage == EBattleStage.InRoom)
                ChangeState<ProcedureInRoom>(procedureOwner);
        }

        protected override void OnLeave(IFsm<ProcedureComponent> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            if (_joinRoomForm is > 0)
                GameEntry.UI.CloseUIForm((int)_joinRoomForm);
        }
    }
}