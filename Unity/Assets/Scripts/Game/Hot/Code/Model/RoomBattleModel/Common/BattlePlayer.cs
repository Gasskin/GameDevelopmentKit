using System.Collections.Generic;

namespace Game.Hot
{
    public partial class BattlePlayer
    {
        public int PlayerId;
        public int HeroId;
        public List<int> HandCards = new();
        public List<int> Equips = new();

        public readonly BattleProperty BattleProperty = new();


    }
}