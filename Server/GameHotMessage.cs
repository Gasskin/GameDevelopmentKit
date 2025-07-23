// This is an automatically generated class by Share.Tool. Please do not modify it.

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Game.Hot
{
    // 心跳
    // proto file : GameHot/GameMsg.proto (line:4)
    [Serializable, ProtoContract(Name = @"CS_PingReq")]
    public partial class CS_PingReq : CSPacketBase
    {
        public override int Id => 30001;
        public override void Clear()
        {
        }
    }

    // proto file : GameHot/GameMsg.proto (line:9)
    [Serializable, ProtoContract(Name = @"SC_PingAck")]
    public partial class SC_PingAck : SCPacketBase
    {
        public override int Id => 30002;
        [ProtoMember(1)]
        public long timeStamp { get; set; }
        public override void Clear()
        {
            this.timeStamp = default;
        }
    }

    // 加入房间
    // proto file : GameHot/GameMsg.proto (line:15)
    [Serializable, ProtoContract(Name = @"CS_JoinRoomReq")]
    public partial class CS_JoinRoomReq : CSPacketBase
    {
        public override int Id => 30003;
        [ProtoMember(1)]
        public int accountId { get; set; }
        public override void Clear()
        {
            this.accountId = default;
        }
    }

    // proto file : GameHot/GameMsg.proto (line:20)
    [Serializable, ProtoContract(Name = @"SC_JoinRoomAck")]
    public partial class SC_JoinRoomAck : SCPacketBase
    {
        public override int Id => 30004;
        [ProtoMember(1)]
        public int myAccountId { get; set; }
        [ProtoMember(2)]
        public List<int> roomPlayers { get; set; } = new List<int>();
        public override void Clear()
        {
            this.myAccountId = default;
            this.roomPlayers.Clear();
        }
    }

    // 广播 玩家加入房间
    // proto file : GameHot/GameMsg.proto (line:27)
    [Serializable, ProtoContract(Name = @"SC_JoinRoomNtf")]
    public partial class SC_JoinRoomNtf : SCPacketBase
    {
        public override int Id => 30005;
        [ProtoMember(1)]
        public int newAccountId { get; set; }
        [ProtoMember(2)]
        public List<int> roomPlayers { get; set; } = new List<int>();
        public override void Clear()
        {
            this.newAccountId = default;
            this.roomPlayers.Clear();
        }
    }

    // 广播 玩家离开房间
    // proto file : GameHot/GameMsg.proto (line:34)
    [Serializable, ProtoContract(Name = @"SC_LeaveRoomNtf")]
    public partial class SC_LeaveRoomNtf : SCPacketBase
    {
        public override int Id => 30006;
        [ProtoMember(1)]
        public int leaveAccountId { get; set; }
        public override void Clear()
        {
            this.leaveAccountId = default;
        }
    }

    // 开始战斗
    // proto file : GameHot/GameMsg.proto (line:40)
    [Serializable, ProtoContract(Name = @"CS_BeginBattleNtf")]
    public partial class CS_BeginBattleNtf : CSPacketBase
    {
        public override int Id => 30007;
        public override void Clear()
        {
        }
    }

    // 广播 开始战斗
    // proto file : GameHot/GameMsg.proto (line:46)
    [Serializable, ProtoContract(Name = @"SC_BeginBattleNtf")]
    public partial class SC_BeginBattleNtf : SCPacketBase
    {
        public override int Id => 30008;
        /// <summary>
        /// 玩家ID，可选择武将ID
        /// </summary>
        [ProtoMember(1)]
        public List<int> canChooseHero { get; set; } = new List<int>();
        /// <summary>
        /// 截止时间戳
        /// </summary>
        [ProtoMember(2)]
        public long endTimestampMs { get; set; }
        /// <summary>
        /// 总操作时长
        /// </summary>
        [ProtoMember(3)]
        public int totalTimeMs { get; set; }
        public override void Clear()
        {
            this.canChooseHero.Clear();
            this.endTimestampMs = default;
            this.totalTimeMs = default;
        }
    }

    // 选择武将
    // proto file : GameHot/GameMsg.proto (line:54)
    [Serializable, ProtoContract(Name = @"CS_ChooseHeroNtf")]
    public partial class CS_ChooseHeroNtf : CSPacketBase
    {
        public override int Id => 30009;
        /// <summary>
        /// 选择的武将ID
        /// </summary>
        [ProtoMember(1)]
        public int heroId { get; set; }
        public override void Clear()
        {
            this.heroId = default;
        }
    }

    public static partial class GameHotMessageId
    {
         public const ushort CS_PingReq = 30001;
         public const ushort SC_PingAck = 30002;
         public const ushort CS_JoinRoomReq = 30003;
         public const ushort SC_JoinRoomAck = 30004;
         public const ushort SC_JoinRoomNtf = 30005;
         public const ushort SC_LeaveRoomNtf = 30006;
         public const ushort CS_BeginBattleNtf = 30007;
         public const ushort SC_BeginBattleNtf = 30008;
         public const ushort CS_ChooseHeroNtf = 30009;
    }
}
