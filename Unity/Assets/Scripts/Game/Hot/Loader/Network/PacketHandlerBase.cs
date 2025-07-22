//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using UnityGameFramework.Extension;

namespace Game
{
    public abstract class PacketHandlerBase : IPacketHandler
    {
        public abstract int Id { get; }

        protected virtual bool HasAck => false;
        
        public void Handle(object sender, Packet packet)
        {
            DoHandle(sender, packet);
            if (HasAck) 
            {
                var e = ReferencePool.Acquire<PacketHandleEventArgs>();
                e.FromReqId = Id;
                e.Packet = packet;
                GameEntry.Event.FireNow(this, e);
            }
            // packet会在这里被自动释放，因为Handle也是Event.Fire过来的
        }

        protected abstract void DoHandle(object sender, Packet packet);
    }
}