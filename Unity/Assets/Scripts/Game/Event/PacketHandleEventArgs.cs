using GameFramework.Event;

namespace Game
{
    public class PacketHandleEventArgs: GameEventArgs
    {
        public static readonly int EventId = typeof(PacketHandleEventArgs).GetHashCode();

        public override int Id => EventId;
        
        public int AckId { get; set; }
        
        public override void Clear()
        {
            AckId = 0;
        }
    }
}

