using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<Game.Hot.ProcedureComponent>;

namespace Game.Hot
{
    public class ProcedureInRoom : ProcedureBase
    {
        private int? _roomForm;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccessEvent);

            _roomForm = GameEntry.UI.OpenUIForm(UIFormId.RoomForm);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccessEvent);
        }

        private void OnOpenUIFormSuccessEvent(object sender, GameEventArgs e)
        {
            var ee = (OpenUIFormSuccessEventArgs)e;
            if (ee.UIForm.SerialId == _roomForm)
            {
                var joinRoomForm = GameEntry.UI.GetUIForm(UIFormId.JoinRoomForm, "Default");
                if (joinRoomForm != null)
                {
                    GameEntry.UI.CloseUIForm(joinRoomForm);
                }
            }
        }
    }
}