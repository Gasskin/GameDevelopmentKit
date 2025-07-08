using GameFramework.Fsm;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public sealed class ProcedureLaunch : ProcedureBase
    {
        protected override void OnEnter(IFsm<ProcedureComponent> procedureOwner)
        {
            base.OnEnter(procedureOwner);
        }

        protected override void OnUpdate(IFsm<ProcedureComponent> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            GameEntry.Network.CreateNetworkChannel("Socket", GameFramework.Network.ServiceType.Tcp, new NetworkChannelHelperHot());
            ChangeState<ProcedurePreload>(procedureOwner);
        }
    }
}
