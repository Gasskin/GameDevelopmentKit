using GameFramework.Event;

namespace Game.Hot
{
    public class RoomPlayerChangeEvent: GameEventArgs
    {
        public static readonly int EventId = typeof(RoomPlayerChangeEvent).GetHashCode();
        
        public override int Id => EventId;
        
        public override void Clear()
        {
        }
    }
}

