using cfg;
using Game.Hot;

public class BattlePlayer
{
    public int PlayerId;
    public int HeroId;
    public List<DRCardPile> HandCards = new();


    public DS_PlayerInitInfo ToPlayerInitInfo()
    {
        var info = new DS_PlayerInitInfo();
        info.heroId = HeroId;
        info.playerId = PlayerId;
        info.handCards = new List<int>();
        foreach (var card in HandCards)
            info.handCards.Add(card.Id);
        return info;
    }
}