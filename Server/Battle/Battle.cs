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

    private const double FRAME_TIME = 1000.0 / 60;

    private bool _isRunning = true;

    private BattleState _battleState = BattleState.None;

    private WaitForClientReady _waitForClientReady;

    public void Init()
    {
        var stopwatch = new Stopwatch();
        while (_isRunning)
        {
            stopwatch.Restart();
            Update();
            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed.TotalMilliseconds;
            var waitTime = FRAME_TIME - elapsed;
            if (waitTime > 0)
                Thread.Sleep((int)waitTime);
        }
    }

    private void Update()
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

    public void OnStartBattleReq(CS_StartBattleReq deserialize)
    {
        _battleState = BattleState.InitWaitForClientReady;
    }

    public void OnReadyForGameReq(CS_ReadyForGameReq msg)
    {
        if (_battleState == BattleState.WaitForClientRead)
        {
            _waitForClientReady.OnReadyForGameReq(msg);
        }
    }
}