using CodeBind;
using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public partial class LoginForm : AHotUIForm
    {
        private int _accountId;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitBind(gameObject.GetComponent<CSCodeBindMono>());
            sureButton.onClick.AddListener(OnSureButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            _accountId = Utility.Random.GetRandom(1, int.MaxValue);
            accountIdTMPText.text = Utility.Text.Format("用户名：{0}", _accountId);
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
            sureButton.onClick.RemoveListener(OnSureButtonClick);
        }


        private void OnSureButtonClick()
        {
            HotEntry.Model.Account.SetAccountId(_accountId);
            var packet = ReferencePool.Acquire<CS_JoinRoomReq>();
            packet.accountId = _accountId;
            GameEntry.Network.SendTcp(packet);
        }
    }
}