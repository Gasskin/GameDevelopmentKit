using System.Collections.Generic;
using System.Net;
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
            
            GameEntry.Event.Subscribe(NetworkConnectedEventArgs.EventId,((sender, args) =>
            {
                Log.Error("Connected");
                var net = GameEntry.Network.GetNetworkChannel("Socket");
                net.Send(new CSHeartBeatTest()
                {
                    A = new List<int>(){1,2,3},
                    B = "123",
                });
            }));
            
            GameEntry.Network.CreateNetworkChannel("Socket", GameFramework.Network.ServiceType.Tcp, new NetworkChannelHelper());
            var net = GameEntry.Network.GetNetworkChannel("Socket");
            net.Connect(IPAddress.Parse("127.0.0.1"), 9000);
            Log.Error("Start Connect");
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
