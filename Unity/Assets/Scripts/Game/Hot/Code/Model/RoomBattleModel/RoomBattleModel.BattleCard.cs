using System.Collections.Generic;

namespace Game.Hot
{
    public partial class RoomBattleModel
    {
        private readonly Dictionary<int, BattleCard> _idToBattleCard = new();

        public BattleCard GetBattleCard(int cardId)
        {
            return _idToBattleCard.GetValueOrDefault(cardId, null);
        }
    }
}