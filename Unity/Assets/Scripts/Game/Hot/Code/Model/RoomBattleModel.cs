using System.Collections.Generic;
using UnityGameFramework.Extension;

namespace Game.Hot
{
    public class SelectHeroStage
    {
        public long EndTimestampMs { get; private set; }
        public int TotalTimeMs { get; private set; }

        private List<int> _canChooseHero;

        public SelectHeroStage(SC_BeginBattleNtf msg)
        {
            _canChooseHero = new List<int>(msg.canChooseHero);
            EndTimestampMs = msg.endTimestampMs;
            TotalTimeMs = msg.totalTimeMs;
        }

        public void GetCanChooseHero(UGFList<int> result)
        {
            result.Clear();
            result.AddRange(_canChooseHero);
        }
    }
    
    public class RoomBattleModel
    {
        public EBattleStage BattleStage { get; private set; } = EBattleStage.None;

        public EBattleStage LoadingStage { get; private set; } = EBattleStage.None;

        public SelectHeroStage SelectHeroStage { get; private set; }
        
        
        public void SC_BeginBattleNtf(SC_BeginBattleNtf msg)
        {
            SelectHeroStage = new(msg);
            BattleStage = EBattleStage.SelectHero;
        }
        
        // public void ChangeRoomBattleStage(EBattleStage stage)
        // {
        //     BattleStage = stage;
        //     if (stage == EBattleStage.None)
        //     {
        //         SelectHeroStage = null;
        //     }
        // }

        public void ChangeRoomBattleLoadingStage(EBattleStage stage)
        {
            LoadingStage = stage;
        }
    }
}