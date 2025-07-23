using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<Game.Hot.ProcedureComponent>;

namespace Game.Hot
{
    public class ProcedureBattleLoading : ProcedureBase
    {
        private int _loadingForm;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            HotEntry.Model.RoomBattle.ChangeRoomBattleLoadingStage(EBattleStage.Loading);
            _loadingForm = GameEntry.UI.OpenUIForm(UIFormId.LoadingForm) ?? 0;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (HotEntry.Model.RoomBattle.LoadingStage == EBattleStage.LoadEnd)
            {
                switch (HotEntry.Model.RoomBattle.BattleStage)
                {
                    case EBattleStage.SelectIdentity:
                        break;
                    case EBattleStage.SelectHero:
                        ChangeState<ProcedureBattleSelectHero>(procedureOwner);
                        break;
                    case EBattleStage.ChangeHandCard:
                        break;
                    case EBattleStage.InBattle:
                        break;
                }
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            if (_loadingForm > 0)
                GameEntry.UI.CloseUIForm(_loadingForm);
        }
    }
}