using System.Net.Sockets;
using Game.Hot;

public class GameMessageHandler
{
    public void Register(MessageDispatcher dispatcher)
    {
        dispatcher.Register<CS_PingReq>(GameHotMessageId.CS_PingReq, OnPingReq);
        dispatcher.Register<CS_JoinRoomReq>(GameHotMessageId.CS_JoinRoomReq, OnJoinRoomReq);
        dispatcher.Register<CS_BeginBattleNtf>(GameHotMessageId.CS_BeginBattleNtf, OnBeginBattleNtf);
    }

    private void OnPingReq(Socket client, CS_PingReq msg)
    {
        Server.Instance.Send(client, new SC_PingAck
        {
            timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        });
    }

    private void OnJoinRoomReq(Socket client, CS_JoinRoomReq msg)
    {
        Room.Instance.JoinRoomReq(client, msg);
    }

    private void OnBeginBattleNtf(Socket client, CS_BeginBattleNtf msg)
    {
        Room.Instance.OnBeginBattleNtf();
        Battle.Instance.OnBeginBattleNtf(msg); // 主线程调用，不需要Post
    }
}