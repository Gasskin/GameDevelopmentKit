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

    // 加入房间请求
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

    // 玩家加入房间广播
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

    // 玩家离开房间广播
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
    [Serializable, ProtoContract(Name = @"CS_StartBattleReq")]
    public partial class CS_StartBattleReq : CSPacketBase
    {
        public override int Id => 30007;
        public override void Clear()
        {
        }
    }

    // proto file : GameHot/GameMsg.proto (line:45)
    [Serializable, ProtoContract(Name = @"SC_StartBattleNtf")]
    public partial class SC_StartBattleNtf : SCPacketBase
    {
        public override int Id => 30008;
        public override void Clear()
        {
        }
    }

    // proto file : GameHot/GameMsg.proto (line:50)
    [Serializable, ProtoContract(Name = @"CS_ReadyForGameNtf")]
    public partial class CS_ReadyForGameNtf : CSPacketBase
    {
        public override int Id => 30009;
        [ProtoMember(1)]
        public int accountId { get; set; }
        public override void Clear()
        {
            this.accountId = default;
        }
    }

}
