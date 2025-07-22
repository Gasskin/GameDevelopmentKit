using System.Net.Sockets;
using cfg;
using Game.Hot;

public class Room
{
    public static Room Instance { get; } = new();

    private Dictionary<int, Socket> _playerToSocket = new();

    private List<int> _roomPlayer = new();

    public void JoinRoomReq(Socket newClient, CS_JoinRoomReq msg)
    {
        Console.WriteLine($"[房间]{msg.accountId}加入房间");
        _playerToSocket.Add(msg.accountId, newClient);

        foreach (var c in _playerToSocket.Values)
        {
            if (c == newClient)
                continue;
            Server.Instance.Send(c, new SC_JoinRoomNtf()
            {
                newAccountId = msg.accountId,
                roomPlayers = _roomPlayer,
            });
        }

        _roomPlayer.Add(msg.accountId);

        var ack = new SC_JoinRoomAck();
        ack.roomPlayers = _roomPlayer;
        ack.myAccountId = msg.accountId;
        Server.Instance.Send(newClient, ack);
    }

    public void OnDisconnect(Socket client)
    {
        var player = 0;
        foreach (var kv in _playerToSocket)
        {
            if (kv.Value == client)
            {
                player = kv.Key;
                break;
            }
        }

        if (player > 0)
        {
            Console.WriteLine($"[房间]{player}离开房间");
            _roomPlayer.Remove(player);
        }
    }

    public List<int> GetPlayers()
    {
        var players = new List<int>(_roomPlayer);
        return players;
    }

    public void OnBeginBattleReq()
    {
        var idx = 0;
        var dtHero = Tables.Instance.DTHero;
        var list = new List<int>();
        foreach (var client in _playerToSocket.Values)
        {
            list.Clear();
            list.Add(dtHero.DataList[idx].Id);
            list.Add(dtHero.DataList[idx + 1].Id);
            Server.Instance.Send(client, new SC_BeginBattleNtf()
            {
                canChooseHero = list,
                endTimestampMs = Program.ServerTimeMs + 30 * 1000,
                totalTimeMs = 30 * 1000,// 30秒
            });
            idx += 2;
        }
    }
}