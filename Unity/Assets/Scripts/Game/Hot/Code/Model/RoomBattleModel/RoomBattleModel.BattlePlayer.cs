using System.Collections.Generic;

namespace Game.Hot
{
    public partial class RoomBattleModel
    {
        public int MainPlayerId { get; private set; }
        private List<BattlePlayer> _players;

        public void StartBattleNtf(SC_StartBattleNtf msg)
        {
            MainPlayerId = msg.mainPlayerId;
            _players = new();
            foreach (var info in msg.playerInitInfos)
                _players.Add(new BattlePlayer(info));
            _players.Sort(((p1, p2) =>
            {
                if (p1.PlayerId == MainPlayerId)
                    return 0;
                return 1;
            }));

            BattleStage = EBattleStage.InBattle;
        }

        public BattlePlayer GetPlayerByIndex(int index)
        {
            if (_players == null || index >= _players.Count)
                return null;
            return _players[index];
        }
    }
}