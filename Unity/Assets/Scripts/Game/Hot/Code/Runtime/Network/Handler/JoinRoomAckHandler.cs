using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public class JoinRoomAckEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(JoinRoomAckEventArgs).GetHashCode();

        public override int Id => EventId;
        
        public override void Clear()
        {
        }
    }
    
    public class JoinRoomAckHandler : PacketHandlerBase
    {
        public override int Id => 30004;

        public override void Handle(object sender, Packet packet)
        {
            var msg = (SC_JoinRoomAck)packet;
            HotEntry.Model.Room.SetRoomPlayers(msg.roomPlayers);

            GameEntry.Event.Fire(this, ReferencePool.Acquire<JoinRoomAckEventArgs>());
        }
    }
}