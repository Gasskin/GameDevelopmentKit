using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using UnityGameFramework.Extension;
using UnityGameFramework.Runtime;

namespace Game
{
    public class NetContainer : IReference
    {
        public static NetContainer Create(string channel)
        {
            NetContainer net = ReferencePool.Acquire<NetContainer>();
            net.m_CancellationTokenSource = new();
            net.m_ChannelName = channel;
            net.SubscribeEvent();
            return net;
        }

        private CancellationTokenSource m_CancellationTokenSource;

        private string m_ChannelName;

        private Dictionary<int, AutoResetUniTaskCompletionSource<Packet>> m_PacketToTcs = new();

        public void Clear()
        {
            if (m_CancellationTokenSource != null)
                m_CancellationTokenSource.Cancel();
            m_CancellationTokenSource = null;
            foreach (var tcs in m_PacketToTcs.Values)
                tcs.TrySetCanceled();
            m_PacketToTcs.Clear();
            UnSubscribeEvent();
        }

        public async UniTask<Packet> SendPacketAsync(Packet packet)
        {
            if (m_PacketToTcs.ContainsKey(packet.Id))
            {
                return await UniTask.FromException<Packet>(new Exception("The packet is already sent"));
            }
            if (m_CancellationTokenSource.IsCancellationRequested)
            {
                return await UniTask.FromCanceled<Packet>(m_CancellationTokenSource.Token);
            }
            var channel = GameEntry.Network.GetNetworkChannel(m_ChannelName);
            if (channel == null)
            {
                return await UniTask.FromException<Packet>(new Exception("Channel not found"));
            }
            var tcs = AutoResetUniTaskCompletionSource<Packet>.Create();
            m_PacketToTcs.Add(packet.Id, tcs);
            
            channel.Send(packet);
            var timeout = UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: m_CancellationTokenSource.Token);
            var (hasCompleted, p) = await UniTask.WhenAny(tcs.Task, timeout);
            if (hasCompleted)
                return p;
            if (m_CancellationTokenSource.IsCancellationRequested)
                return await UniTask.FromCanceled<Packet>(m_CancellationTokenSource.Token);
            return await UniTask.FromException<Packet>(new TimeoutException("Request timeout"));
        }


        private void SubscribeEvent()
        {
            GameEntry.Event.Subscribe(PacketHandleEventArgs.EventId, OnPacketHandleEvent);
        }

        private void UnSubscribeEvent()
        {
            GameEntry.Event.Unsubscribe(PacketHandleEventArgs.EventId, OnPacketHandleEvent);
        }

        private void OnPacketHandleEvent(object sender, GameEventArgs e)
        {
            var ee = (PacketHandleEventArgs)e;
            if (!m_PacketToTcs.ContainsKey(ee.FromReqId))
                return;
            if (m_PacketToTcs.Remove(ee.FromReqId, out var tcs))
            {
                if (m_CancellationTokenSource.IsCancellationRequested)
                {
                    tcs.TrySetCanceled(m_CancellationTokenSource.Token);
                    return;
                }
                tcs.TrySetResult(ee.Packet);
            }
        }
    }
}