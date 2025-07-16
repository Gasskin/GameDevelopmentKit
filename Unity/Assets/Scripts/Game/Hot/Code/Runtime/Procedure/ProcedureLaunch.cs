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
        protected override void OnUpdate(IFsm<ProcedureComponent> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            ChangeState<ProcedureLogin>(procedureOwner);
        }
    }
}