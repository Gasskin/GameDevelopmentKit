using System.Net.Sockets;
using Game.Hot;

public class GameMessageHandler
{
    public void Register(MessageDispatcher dispatcher)
    {
        dispatcher.Register<CS_PingReq>(GameHotMessageId.CS_PingReq, OnPingReq);
        dispatcher.Register<CS_JoinRoomReq>(GameHotMessageId.CS_JoinRoomReq, OnJoinRoomReq);
        dispatcher.Register<CS_BeginBattleNtf>(GameHotMessageId.CS_BeginBattleNtf, OnBeginBattleNtf);
        dispatcher.Register<CS_ChooseHeroReq>(GameHotMessageId.CS_ChooseHeroReq, OnChooseHeroReq);
    }

    private void OnChooseHeroReq(int account, Socket client, CS_ChooseHeroReq msg)
    {
        if (Room.Instance.CanChooseHero(account, msg.heroId))
        {
            Server.Instance.Send(client, new SC_ChooseHeroAck()
            {
                heroId = msg.heroId,
            });
        }
    }

    private void OnPingReq(int account, Socket client, CS_PingReq msg)
    {
        Server.Instance.Send(client, new SC_PingAck
        {
            timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        });
    }

    private void OnJoinRoomReq(int account, Socket client, CS_JoinRoomReq msg)
    {
        Room.Instance.OnJoinRoomReq(client, msg);
    }

    private void OnBeginBattleNtf(int account, Socket client, CS_BeginBattleNtf msg)
    {
        Room.Instance.OnBeginBattleNtf();
        Battle.Instance.OnBeginBattleNtf(msg); // 主线程调用，不需要Post
    }
}