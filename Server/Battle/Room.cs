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
    
    private Dictionary<int,int> _playerToChooseHeroId = new();

    private List<DRCardPile> _cardPile;
    
    private Dictionary<int,BattlePlayer> _playerToBattlePlayer = new();

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
        _playerToChooseHeroId.Clear();
        var randomHero = RandomList(Tables.Instance.DTHero.DataList);
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
            Server.Instance.Send(pair.Value, new SC_StartChooseHeroNtf()
            {
                canChooseHero = _canChooseHeroId[pair.Key],
                endTimestampMs = Program.ServerTimeMs + 20 * 1000,
                totalTimeMs = 20 * 1000, // 30秒
            });
            idx += 3;
        }
    }
    
    
    public void OnChooseHeroReq(int account, CS_ChooseHeroReq msg)
    {
        _playerToChooseHeroId.TryAdd(account, 0);
        _playerToChooseHeroId[account]=msg.heroId;
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

    public bool IsSelectHeroEnd()
    {
        return _playerToChooseHeroId.Count == 2;
    }

    public void StartBattle()
    {
        _playerToBattlePlayer.Clear();
        _cardPile = RandomList(Tables.Instance.DTCardPile.DataList);
        foreach (var pair in _playerToChooseHeroId)
        {
            var bp = new BattlePlayer();
            bp.HeroId = pair.Value;
            bp.PlayerId = pair.Key;
            var drHero = Tables.Instance.DTHero[pair.Value];
            for (int i = 0; i < drHero.HandLimit; i++)
            {
                var card = _cardPile[^1];
                bp.HandCards.Add(card);
                _cardPile.RemoveAt(_cardPile.Count - 1);
            }
            _playerToBattlePlayer.Add(pair.Key, bp);
        }

        var infoList = new List<DS_PlayerInitInfo>();
        foreach (var pair in _playerToSocket)
        {
            infoList.Add(_playerToBattlePlayer[pair.Key].ToPlayerInitInfo());
        }
        foreach (var pair in _playerToSocket)
        {
            Server.Instance.Send(pair.Value, new SC_StartBattleNtf()
            {
                playerInitInfos = infoList,
            });
        }
    }

    private List<T> RandomList<T>(List<T> source)
    {
        var result = new List<T>(source);
        var r = new Random();
        for (int i = result.Count - 1; i > 0; i--)
        {
            var rr = r.Next(0, i + 1);
            (result[rr], result[i]) = (result[i], result[rr]);
        }
        return result;
    }
}