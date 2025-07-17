using GameFramework;
using GameFramework.Event;
using GameFramework.Network;

namespace Game
{
    public class PacketHandleEventArgs: GameEventArgs
    {
        public static readonly int EventId = typeof(PacketHandleEventArgs).GetHashCode();

        public override int Id => EventId;
        
        public int FromReqId { get; set; }
        
        // 会被自动释放
        public Packet Packet { get; set; }
        
        public override void Clear()
        {
            FromReqId = 0;
            Packet = null;
        }
    }
}

