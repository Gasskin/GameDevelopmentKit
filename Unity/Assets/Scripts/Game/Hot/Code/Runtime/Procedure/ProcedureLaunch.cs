using System.Net;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<Game.Hot.ProcedureComponent>;

namespace Game.Hot
{
    public sealed class ProcedureLaunch : ProcedureBase
    {
        protected override void OnEnter(IFsm<ProcedureComponent> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnected);
            GameEntry.Network.CreateNetworkChannel("TcpChannel", GameFramework.Network.ServiceType.Tcp, new NetworkChannelHelperHot());
            GameEntry.Network.GetNetworkChannel("TcpChannel").Connect(IPAddress.Parse("127.0.0.1"), 12388);
        }

        protected override void OnUpdate(IFsm<ProcedureComponent> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            // ChangeState<ProcedurePreload>(procedureOwner);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnected);
        }

        private void OnNetworkConnected(object sender, GameEventArgs e)
        {
            GameEntry.UI.OpenUIForm(UIFormId.LoginForm);
        }
    }
}