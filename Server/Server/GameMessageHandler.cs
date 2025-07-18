using System.Net.Sockets;
using Game.Hot;

public class GameMessageHandler
{
    public void Register(MessageDispatcher dispatcher)
    {
        dispatcher.Register<CS_PingReq>(CS_PingReq.MsgId, OnPingReq);
        dispatcher.Register<CS_JoinRoomReq>(CS_JoinRoomReq.MsgId, OnJoinRoomReq);
        dispatcher.Register<CS_StartBattleReq>(CS_StartBattleReq.MsgId, OnStartBattleReq);
        dispatcher.Register<CS_ReadyForGameNtf>(CS_ReadyForGameNtf.MsgId, OnReadyForGameNtf);
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

    private void OnStartBattleReq(Socket client, CS_StartBattleReq msg)
    {
        Room.Instance.OnStartBattleReq();
        Battle.Instance.OnStartBattleReq(msg); // 主线程调用，不需要Post
    }

    private void OnReadyForGameNtf(Socket client, CS_ReadyForGameNtf msg)
    {
        Battle.Instance.OnReadyForGameReq(msg);
    }
}