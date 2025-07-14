using System.Collections.Generic;
using System.Net;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<Game.Hot.ProcedureComponent>;

namespace Game.Hot
{
    public sealed class ProcedureLaunch : ProcedureBase
    {
        protected override void OnEnter(IFsm<ProcedureComponent> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(NetworkConnectedEventArgs.EventId, ((sender, args) =>
            {
                var packet = ReferencePool.Acquire<CS_JoinRoomReq>();
                packet.accountId = Utility.Random.GetRandom(1, int.MaxValue);
                GameEntry.Network.SendTcp(packet);
            }));

            GameEntry.Network.CreateNetworkChannel("TcpChannel", GameFramework.Network.ServiceType.Tcp, new NetworkChannelHelperHot());
            var net = GameEntry.Network.GetNetworkChannel("TcpChannel");
            net.Connect(IPAddress.Parse("127.0.0.1"), 12388);
        }

        protected override void OnUpdate(IFsm<ProcedureComponent> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            // ChangeState<ProcedurePreload>(procedureOwner);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }
    }
}