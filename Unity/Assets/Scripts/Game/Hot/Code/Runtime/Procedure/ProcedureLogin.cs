using GameFramework.Event;
using ProcedureOwner = GameFramework.Fsm.IFsm<Game.Hot.ProcedureComponent>;

namespace Game.Hot
{
    public class ProcedureLogin : ProcedureBase
    {
        private int _loginFormId;
        
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            _loginFormId = GameEntry.UI.OpenUIForm(UIFormId.LoginForm) ?? 0;
            GameEntry.Event.Subscribe(JoinRoomAckEventArgs.EventId, OnJoinRoomAckEvent);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(JoinRoomAckEventArgs.EventId, OnJoinRoomAckEvent);
        }

        private void OnJoinRoomAckEvent(object sender, GameEventArgs e)
        {
            if (_loginFormId > 0)
                GameEntry.UI.CloseUIForm(_loginFormId);
            GameEntry.UI.OpenUIForm(UIFormId.RoomForm);
        }
    }
}