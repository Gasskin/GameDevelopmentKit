namespace Game.Hot
{
    public partial class BattleCard
    {
        public int CardId { get; private set; }

        public int ConfigId { get; private set; }

        public int Num { get; private set; }

        public ECardAttr CardAttr { get; private set; }

        public BattleProperty BattleProperty = new();


        public BattleCard(int cardId, int configId, int num, ECardAttr cardAttr)
        {
            CardId = cardId;
            ConfigId = configId;
            Num = num;
            CardAttr = cardAttr;
        }
    }
}