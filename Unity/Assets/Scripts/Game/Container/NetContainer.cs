using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using UnityGameFramework.Extension;

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
        private List<int> m_PacketSending = new();
        private List<int> m_PacketReceived = new();

        public void Clear()
        {
            if (m_CancellationTokenSource != null)
                m_CancellationTokenSource.Cancel();
            m_CancellationTokenSource = null;
            UnSubscribeEvent();
        }

        public UniTask<T> SendPacketAsync<T>(Packet packet) where T : Packet
        {
            if (m_PacketSending.Contains(packet.Id))
            {
                return UniTask.FromException<T>(new Exception("The packet is already sent, but no receive"));
            }
            if (m_CancellationTokenSource.IsCancellationRequested)
            {
                return UniTask.FromCanceled<T>(m_CancellationTokenSource.Token);
            }
            var channel = GameEntry.Network.GetNetworkChannel(m_ChannelName);
            if (channel == null)
            {
                return UniTask.FromException<T>(new Exception("Channel not found"));
            }
            channel.Send(packet);
            m_PacketSending.Add(packet.Id);
            // var delayOneFrame = true;
            bool MoveNext(ref UniTaskCompletionSourceCore<T> core)
            {
                if (!m_PacketReceived.Contains(packet.Id))
                    return true;
                //等待一帧GF的Event.Fire
                // if (delayOneFrame)
                    // delayOneFrame = false;
                m_PacketSending.Remove(packet.Id);
                m_PacketReceived.Remove(packet.Id);
                return false;
            }

            return Awaitable.NewUniTask<T>(MoveNext, m_CancellationTokenSource.Token);
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
            m_PacketReceived.Add(ee.AckId);
        }
    }
}