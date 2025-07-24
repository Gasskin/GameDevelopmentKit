using System.Net;
using CodeBind;
using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using UnityGameFramework.Extension;
using UnityGameFramework.Runtime;
using NetworkConnectedEventArgs = UnityGameFramework.Runtime.NetworkConnectedEventArgs;

namespace Game.Hot
{
    public partial class JoinRoomForm : AHotUIForm
    {
        private int _accountId;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            InitBind(gameObject.GetComponent<CSCodeBindMono>());
            _accountId = 0;
            AccountIdTMPText.text = "等待连接...";
            SureButton.gameObject.SetActive(false);
            SureButton.onClick.AddListener(JoinRoomReq);
            Subscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnectedEvent);
            GameEntry.Network.CreateTcpChannel();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            SureButton.onClick.RemoveAllListeners();
            ClearBind();
        }

        private void JoinRoomReq()
        {
            if (_accountId <= 0) 
                return;
            var packet = ReferencePool.Acquire<CS_JoinRoomReq>();
            packet.accountId = _accountId;
            GameEntry.Network.SendTcp(packet);
        }

        private void OnNetworkConnectedEvent(object sender, GameEventArgs e)
        {
            _accountId = Utility.Random.GetRandom(1, int.MaxValue);
            AccountIdTMPText.text = Utility.Text.Format("用户名：{0}", _accountId);
            SureButton.gameObject.SetActive(true);
        }
    }
}