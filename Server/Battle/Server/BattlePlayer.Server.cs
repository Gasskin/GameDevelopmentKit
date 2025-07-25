namespace Game.Hot
{
    public partial class BattlePlayer
    {
        public DS_PlayerInitInfo ToPlayerInitInfo()
        {
            var info = new DS_PlayerInitInfo();
            info.heroId = HeroId;
            info.playerId = PlayerId;
            info.handCards = new(HandCards);
            info.properties = new();
            return info;
        }
    }
}