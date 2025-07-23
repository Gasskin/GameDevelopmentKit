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
            _playerToSocket.Remove(player);
        }
    }

    public List<int> GetPlayers()
    {
        var players = new List<int>(_roomPlayer);
        return players;
    }

    public void OnBeginBattleNtf()
    {
        var randomHero = new List<DRHero>(Tables.Instance.DTHero.DataList);
        var r = new Random();
        for (int i = randomHero.Count - 1; i > 0; i--)
        {
            var rr = r.Next(0, i + 1);
            (randomHero[rr], randomHero[i]) = (randomHero[i], randomHero[rr]);
        }
        var sendList = new List<int>();
        var idx = 0;
        foreach (var client in _playerToSocket.Values)
        {
            sendList.Clear();
            for (int i = idx; i < idx + 3; i++)
                sendList.Add(randomHero[i].Id);
            Server.Instance.Send(client, new SC_BeginBattleNtf()
            {
                canChooseHero = sendList,
                endTimestampMs = Program.ServerTimeMs + 20 * 1000,
                totalTimeMs = 20 * 1000, // 30秒
            });
            idx += 3;
        }
    }
}