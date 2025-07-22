using ProcedureOwner = GameFramework.Fsm.IFsm<Game.Hot.ProcedureComponent>;

namespace Game.Hot
{
    public class ProcedureSelectHero : ProcedureBase
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
        }
    }
}