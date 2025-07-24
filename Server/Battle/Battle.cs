using System.Diagnostics;
using Game.Hot;

public enum BattleState
{
    None = 0,
    WaitSelectHero,
    InBattle,
}

public class Battle
{
    public static Battle Instance { get; } = new();

    private BattleState _battleState = BattleState.None;

    private WaitSelectHeroStage _waitSelectHeroStage = new();

    public void Init()
    {

    }

    public void Update()
    {
        switch (_battleState)
        {
            case BattleState.None:
                break;
            case BattleState.WaitSelectHero:
                if (!_waitSelectHeroStage.IsSelectHeroEnd())
                    return;
                Room.Instance.StartBattle();
                _battleState = BattleState.InBattle;
                break;
            case BattleState.InBattle:
                break;
        }
    }

    public void OnBeginBattleNtf(CS_BeginBattleNtf msg)
    {
        _battleState = BattleState.WaitSelectHero;
    }
}