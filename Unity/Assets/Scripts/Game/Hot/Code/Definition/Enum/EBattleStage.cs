namespace Game.Hot
{
    public enum EBattleStage
    {
        None = 0,
        // 房间中
        InRoom,
        // 资源加载中
        Loading,
        // 资源加载结束
        LoadEnd,
        // 选择武将
        SelectHero,
        // 战斗中
        Battling
    }
}