using ProcedureOwner = GameFramework.Fsm.IFsm<Game.Hot.ProcedureComponent>;

namespace Game.Hot
{
    public class ProcedureInRoom : ProcedureBase
    {
        private int? _roomForm;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            _roomForm = GameEntry.UI.OpenUIForm(UIFormId.RoomForm);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (HotEntry.Model.Room.BattleStage == EBattleStage.Loading)
            {
                ChangeState<ProcedureLoading>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            if (_roomForm is > 0)
                GameEntry.UI.CloseUIForm((int)_roomForm);
        }
    }
}