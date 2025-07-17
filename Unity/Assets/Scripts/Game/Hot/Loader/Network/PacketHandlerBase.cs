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
        
        public virtual int AckId { get; }

        public void Handle(object sender, Packet packet)
        {
            DoHandle(sender, packet);
            if (AckId > 0) 
            {
                var e = ReferencePool.Acquire<PacketHandleEventArgs>();
                e.AckId = AckId;
                GameEntry.Event.Fire(this, e);
            }
        }

        protected abstract void DoHandle(object sender, Packet packet);
    }
}