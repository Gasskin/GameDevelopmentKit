using System.Net.Sockets;
using Game.Hot;

public class GameMessageHandler
{
    public void Register(MessageDispatcher dispatcher)
    {
        dispatcher.Register<CS_PingReq>(CS_PingReq.MsgId, OnPingReq);
        dispatcher.Register<CS_JoinRoomReq>(CS_JoinRoomReq.MsgId, OnJoinRoomReq);
        dispatcher.Register<CS_BeginBattleReq>(CS_BeginBattleReq.MsgId, OnBeginBattleReq);
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

    private void OnBeginBattleReq(Socket client, CS_BeginBattleReq msg)
    {
        Room.Instance.OnBeginBattleReq();
        Battle.Instance.OnBeginBattleReq(msg); // 主线程调用，不需要Post
    }
}