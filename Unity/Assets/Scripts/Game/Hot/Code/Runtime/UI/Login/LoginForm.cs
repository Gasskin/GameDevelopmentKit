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
    public partial class LoginForm : AHotUIForm
    {
        private int _accountId;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitBind(gameObject.GetComponent<CSCodeBindMono>());
            SureButton.onClick.AddListener(OnSureButtonClick);
            GameEntry.Event.Subscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnectedEvent);
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
            SureButton.onClick.RemoveListener(OnSureButtonClick);
            GameEntry.Event.Unsubscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnectedEvent);
        }


        private void OnSureButtonClick()
        {
            // HotEntry.Model.Account.SetAccountId(_accountId);
            // var packet = ReferencePool.Acquire<CS_JoinRoomReq>();
            // packet.accountId = _accountId;
            // GameEntry.Network.SendTcp(packet);
            JoinRoomAsync().Forget();
        }

        private async UniTaskVoid JoinRoomAsync()
        {
            var packet = ReferencePool.Acquire<CS_JoinRoomReq>();
            packet.accountId = _accountId;
            var ack = await SendPacketAsync<SC_JoinRoomAck>(packet);
            Log.Error($"房间人数：{ack.roomPlayers.Count}");
        }

        private void OnNetworkConnectedEvent(object sender, GameEventArgs e)
        {
            _accountId = Utility.Random.GetRandom(1, int.MaxValue);
            AccountIdTMPText.text = Utility.Text.Format("用户名：{0}", _accountId);
            SureButton.gameObject.SetActive(true);
        }
    }
}