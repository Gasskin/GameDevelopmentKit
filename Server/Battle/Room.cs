using System.Net.Sockets;
using Game.Hot;

public class Room
{
    public static Room Instance { get; } = new();

    private Dictionary<int, Socket> _roomPlayer = new();


    public void JoinRoomReq(Socket client, CS_JoinRoomReq msg)
    {
        foreach (var c in _roomPlayer.Values)
        {
            Server.Instance.Send(c, new SC_JoinRoomNtf()
            {
                accountId = msg.accountId,
            });
        }
        _roomPlayer.Add(msg.accountId, client);
        Console.WriteLine($"{msg.accountId}加入房间");
        var ack = new SC_JoinRoomAck();
        foreach (var player in _roomPlayer)
        {
            ack.roomPlayers.Add(player.Key);
        }
        Server.Instance.Send(client, ack);
    }
}