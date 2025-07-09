using GameFramework.Network;
using Game.Hot;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public class SCHeartBeatTestHandler : PacketHandlerBase
    {
        public override int Id => 30002;

        public override void Handle(object sender, Packet packet)
        {
            var data = (SCHeartBeatTest)packet;
            Log.Error("Receive packet '{0}'.", Utility.Json.ToJson(data));
        }
    }
}