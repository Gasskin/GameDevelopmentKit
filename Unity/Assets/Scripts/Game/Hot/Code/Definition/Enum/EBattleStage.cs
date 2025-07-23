namespace Game.Hot
{
    public enum EBattleStage
    {
        None = 0,
        Loading,
        LoadEnd,
        
        // 选择身份
        SelectIdentity,

        // 选择武将
        SelectHero,
        
        // 更换手牌
        ChangeHandCard,

        // 战斗中
        InBattle
    }
}