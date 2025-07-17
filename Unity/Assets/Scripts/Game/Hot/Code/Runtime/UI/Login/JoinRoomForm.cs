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
        private int _roomFormId;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitBind(gameObject.GetComponent<CSCodeBindMono>());
            SureButton.onClick.AddListener((() =>
            {
                JoinRoomAsync().Forget();
            }));
            GameEntry.Event.Subscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnectedEvent);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccessEvent);
        }


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            AccountIdTMPText.text = "等待连接...";
            SureButton.gameObject.SetActive(false);

            GameEntry.Network.CreateNetworkChannel("TcpChannel", GameFramework.Network.ServiceType.Tcp, new NetworkChannelHelperHot());
            GameEntry.Network.GetNetworkChannel("TcpChannel").Connect(IPAddress.Parse("127.0.0.1"), 12388);
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
            SureButton.onClick.RemoveAllListeners();
            GameEntry.Event.Unsubscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnectedEvent);
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccessEvent);
        }

        private async UniTaskVoid JoinRoomAsync()
        {
            var packet = ReferencePool.Acquire<CS_JoinRoomReq>();
            packet.accountId = _accountId;
            await SendPacketAsync<SC_JoinRoomAck>(packet);
            _roomFormId = GameEntry.UI.OpenUIForm(UIFormId.RoomForm) ?? 0;
        }

        private void OnNetworkConnectedEvent(object sender, GameEventArgs e)
        {
            _accountId = Utility.Random.GetRandom(1, int.MaxValue);
            AccountIdTMPText.text = Utility.Text.Format("用户名：{0}", _accountId);
            SureButton.gameObject.SetActive(true);
        }
        
        private void OnOpenUIFormSuccessEvent(object sender, GameEventArgs e)
        {
            var ee = (OpenUIFormSuccessEventArgs)e;
            if (ee.UIForm.SerialId == _roomFormId)
            {
                Close();
            }
        }
    }
}