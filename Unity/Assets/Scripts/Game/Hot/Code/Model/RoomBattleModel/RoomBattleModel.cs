using System.Collections.Generic;
using UnityGameFramework.Extension;

namespace Game.Hot
{
    public class SelectHeroStage
    {
        public long EndTimestampMs { get; private set; }
        public int TotalTimeMs { get; private set; }

        private List<int> _canChooseHero;

        public SelectHeroStage(SC_StartChooseHeroNtf msg)
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
    
    public partial class RoomBattleModel
    {
        public EBattleStage BattleStage { get; private set; } = EBattleStage.None;

        public EBattleStage LoadingStage { get; private set; } = EBattleStage.None;

        public SelectHeroStage SelectHeroStage { get; private set; }
        
        public void StartChooseHeroNtf(SC_StartChooseHeroNtf msg)
        {
            SelectHeroStage = new(msg);
            BattleStage = EBattleStage.ChooseHero;
        }
        
        public void ChangeRoomBattleLoadingStage(EBattleStage stage)
        {
            LoadingStage = stage;
        }
    }
}