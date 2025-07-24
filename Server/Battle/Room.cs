using System.Net.Sockets;
using cfg;
using Game.Hot;

public class Room
{
    public static Room Instance { get; } = new();

    private Dictionary<int, Socket> _playerToSocket = new();

    private Dictionary<Socket, int> _socketToPlayer = new();

    private Dictionary<int, List<int>> _canChooseHeroId = new();

    private List<int> _roomPlayer = new();

    public void OnJoinRoomReq(Socket newClient, CS_JoinRoomReq msg)
    {
        Console.WriteLine($"[房间]{msg.accountId}加入房间");
        _playerToSocket.Add(msg.accountId, newClient);
        _socketToPlayer.Add(newClient, msg.accountId);

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

    public void OnBeginBattleNtf()
    {
        var randomHero = new List<DRHero>(Tables.Instance.DTHero.DataList);
        var r = new Random();
        for (int i = randomHero.Count - 1; i > 0; i--)
        {
            var rr = r.Next(0, i + 1);
            (randomHero[rr], randomHero[i]) = (randomHero[i], randomHero[rr]);
        }
        var idx = 0;
        _canChooseHeroId.Clear();
        foreach (var id in _roomPlayer)
        {
            var heroList = new List<int>();
            for (int i = idx; i < idx + 3; i++)
                heroList.Add(randomHero[i].Id);
            _canChooseHeroId.Add(id, heroList);
            idx += 3;
        }
        foreach (var pair in _playerToSocket)
        {
            Server.Instance.Send(pair.Value, new SC_BeginBattleNtf()
            {
                canChooseHero = _canChooseHeroId[pair.Key],
                endTimestampMs = Program.ServerTimeMs + 20 * 1000,
                totalTimeMs = 20 * 1000, // 30秒
            });
            idx += 3;
        }
    }

    public List<int> GetPlayers()
    {
        var players = new List<int>(_roomPlayer);
        return players;
    }

    public bool CanChooseHero(int playerId, int heroId)
    {
        return _canChooseHeroId.TryGetValue(playerId, out var heroList) && heroList.Contains(heroId);
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
            _socketToPlayer.Remove(client);
        }
    }

    public int SocketToAccount(Socket client)
    {
        return _socketToPlayer.GetValueOrDefault(client, 0);
    }
}