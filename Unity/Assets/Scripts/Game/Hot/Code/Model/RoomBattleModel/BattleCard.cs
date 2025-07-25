namespace Game.Hot
{
    public class BattleCard
    {
        public int CardId { get; private set; }
        
        public int ConfigId { get; private set; }
        
        public int Num { get; private set; }
        
        public ECardAttr CardAttr { get; private set; }
    }
}