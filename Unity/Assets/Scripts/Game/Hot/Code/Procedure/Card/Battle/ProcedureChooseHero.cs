using ProcedureOwner = GameFramework.Fsm.IFsm<Game.Hot.ProcedureComponent>;

namespace Game.Hot
{
    public class ProcedureChooseHero : ProcedureBase
    {
        private int _chooseHeroForm;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            _chooseHeroForm = GameEntry.UI.OpenUIForm(UIFormId.ChooseHeroForm) ?? 0;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            switch (HotEntry.Model.RoomBattle.BattleStage)
            {
                case EBattleStage.InBattle:
                    ChangeState<ProcedureInBattle>(procedureOwner);
                    break;
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            if (_chooseHeroForm > 0)
                GameEntry.UI.CloseUIForm(_chooseHeroForm);
        }
    }
}