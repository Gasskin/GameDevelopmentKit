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
        private int _state;
        private int _loginForm;

        protected override void OnEnter(IFsm<ProcedureComponent> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            _state = 0;
            GameEntry.Event.Subscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnected);
            GameEntry.Network.CreateNetworkChannel("TcpChannel", GameFramework.Network.ServiceType.Tcp, new NetworkChannelHelperHot());
            GameEntry.Network.GetNetworkChannel("TcpChannel").Connect(IPAddress.Parse("127.0.0.1"), 12388);
        }

        protected override void OnUpdate(IFsm<ProcedureComponent> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (_state == 0 && HotEntry.Model.Room.PlayerCount >= 0) 
            {
                if (_loginForm > 0)
                {
                    GameEntry.UI.CloseUIForm(_loginForm);
                    GameEntry.UI.OpenUIForm(UIFormId.RoomForm);
                }
                _state = 1;
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnected);
        }

        private void OnNetworkConnected(object sender, GameEventArgs e)
        {
            _loginForm = GameEntry.UI.OpenUIForm(UIFormId.LoginForm) ?? 0;
        }
    }
}