using System.Collections.Generic;
using CodeBind;
using GameFramework;
using UnityGameFramework.Extension;

namespace Game.Hot
{
    public partial class SelectHeroForm : AHotUIForm
    {
        private List<AUIWidget> _heroList = new();
        
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitBind(gameObject.GetComponent<CSCodeBindMono>());
            AddUIWidget(ProgressBarUIWidget);
            AddUIWidget(HeroCardUIWidget);
            AddUIWidget(HeroCard1UIWidget);
            AddUIWidget(HeroCard2UIWidget);
            AddUIWidget(HeroCard3UIWidget);
            _heroList.Clear();
            _heroList.Add(HeroCard1UIWidget);
            _heroList.Add(HeroCard2UIWidget);
            _heroList.Add(HeroCard3UIWidget);
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
            ClearBind();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            OpenUIWidget(ProgressBarUIWidget, ProgressBarWidgetOpenData.Create(HotEntry.Model.RoomBattle.SelectHeroStage.EndTimestampMs, HotEntry.Model.RoomBattle.SelectHeroStage.TotalTimeMs));
            
            HeroCardUIWidget.gameObject.SetActive(false);
            
            using var canChooseHero = UGFList<int>.Create();
            HotEntry.Model.RoomBattle.SelectHeroStage.GetCanChooseHero(canChooseHero);
            if (canChooseHero.Count > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    var heroId = canChooseHero[i];
                    OpenUIWidget(_heroList[i], HeroCardWidgetOpenData.Create(heroId));
                }
            }
        }
    }
}