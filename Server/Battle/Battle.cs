using System.Diagnostics;
using Game.Hot;

public enum BattleState
{
    None = 0,
    InitWaitForClientReady,
    WaitForClientRead,
    TrueStartBattle
}

public class Battle
{
    public static Battle Instance { get; } = new();



    private BattleState _battleState = BattleState.None;

    private WaitForClientReady _waitForClientReady;

    public void Init()
    {

    }

    public void Update()
    {
        switch (_battleState)
        {
            case BattleState.None:
                break;
            case BattleState.InitWaitForClientReady:
                _waitForClientReady = new(Room.Instance.GetPlayers());
                _battleState = BattleState.WaitForClientRead;
                break;
            case BattleState.WaitForClientRead:
                var allReady = _waitForClientReady.Wait();
                if (allReady)
                {
                    Console.WriteLine("开始战斗！");
                    _battleState = BattleState.TrueStartBattle;
                }
                break;
        }
    }

    public void OnBeginBattleReq(CS_BeginBattleReq deserialize)
    {
        _battleState = BattleState.InitWaitForClientReady;
    }
}