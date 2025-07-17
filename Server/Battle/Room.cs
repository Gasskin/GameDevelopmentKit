using System.Net.Sockets;
using Game.Hot;

public class Room
{
    public static Room Instance { get; } = new();

    private Dictionary<int, Socket> _roomPlayer = new();
    
    public void JoinRoomReq(Socket newClient, CS_JoinRoomReq msg)
    {
        Console.WriteLine($"[房间]{msg.accountId}加入房间");
        _roomPlayer.Add(msg.accountId, newClient);
        var nowPlayers = _roomPlayer.Keys.ToList();
        foreach (var c in _roomPlayer.Values)
        {
            if (c == newClient) 
                continue;
            Server.Instance.Send(c, new SC_JoinRoomNtf()
            {
                newAccountId = msg.accountId,
                roomPlayers = nowPlayers,
            });
        }

        var ack = new SC_JoinRoomAck();
        ack.roomPlayers = nowPlayers;
        ack.myAccountId = msg.accountId;
        Server.Instance.Send(newClient, ack);
    }

    public void OnDisconnect(Socket client)
    {
        int? targetId = null;
        foreach (var kv in _roomPlayer)
        {
            if (kv.Value == client)
            {
                targetId = kv.Key;
                break;
            }
        }

        if (targetId.HasValue)
        {
            Console.WriteLine($"[房间]{targetId.Value}离开房间");
            _roomPlayer.Remove(targetId.Value);
        }
    }

    public List<int> GetPlayers()
    {
        return _roomPlayer.Keys.ToList();
    }

    public void OnStartBattleReq()
    {
        foreach (var client in _roomPlayer.Values)
        {
            Server.Instance.Send(client, new SC_StartBattleNtf()
            {
            });
        }
    }
}