using ProcedureOwner = GameFramework.Fsm.IFsm<Game.Hot.ProcedureComponent>;

namespace Game.Hot
{
    public class ProcedureBattleSelectHero : ProcedureBase
    {
        private int _selectHeroForm;
        
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            _selectHeroForm = GameEntry.UI.OpenUIForm(UIFormId.SelectHeroForm) ?? 0;
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            if (_selectHeroForm > 0)
                GameEntry.UI.CloseUIForm(_selectHeroForm);
        }
    }
}