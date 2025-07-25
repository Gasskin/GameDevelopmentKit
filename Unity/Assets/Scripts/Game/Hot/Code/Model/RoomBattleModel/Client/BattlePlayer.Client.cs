namespace Game.Hot
{
    public partial class BattlePlayer
    {
        public BattlePlayer(DS_PlayerInitInfo info)
        {
            PlayerId = info.playerId;
            HeroId = info.heroId;
            foreach (var card in info.handCards)
                HandCards.Add(card);
            foreach (var property in info.properties)
                BattleProperty.SetPropertyClient(property.propertyType, property.value);
        }
    }
}