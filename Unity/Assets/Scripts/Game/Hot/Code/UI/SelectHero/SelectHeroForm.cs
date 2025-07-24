using System.Collections.Generic;
using CodeBind;
using Cysharp.Threading.Tasks;
using GameFramework;
using UnityGameFramework.Extension;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public partial class SelectHeroForm : AHotUIForm
    {
        private List<HeroCardElement> _heroList = new();
        private HeroCardElement _currentHero;
        private ProgressBarElement _progressBar;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            InitBind(gameObject.GetComponent<CSCodeBindMono>());

            _currentHero = OpenUIElement<HeroCardElement>(HeroCardUIElement);
            _progressBar = OpenUIElement<ProgressBarElement>(ProgressBarUIElement);
            _heroList.Add(OpenUIElement<HeroCardElement>(HeroCard1UIElement));
            _heroList.Add(OpenUIElement<HeroCardElement>(HeroCard2UIElement));
            _heroList.Add(OpenUIElement<HeroCardElement>(HeroCard3UIElement));
            _currentHero.Visible = false;

            var stage = HotEntry.Model.RoomBattle.SelectHeroStage;
            _progressBar.StartProgress(stage.EndTimestampMs, stage.TotalTimeMs);

            using var canChooseHero = UGFList<int>.Create();
            HotEntry.Model.RoomBattle.SelectHeroStage.GetCanChooseHero(canChooseHero);
            if (canChooseHero.Count > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    var heroId = canChooseHero[i];
                    _heroList[i].SetHeroInfo(heroId);
                    _heroList[i].SetCanClick(OnHeroElementClick);
                }
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            _heroList.Clear();
            ClearBind();
        }

        private void OnHeroElementClick(int heroId)
        {
            ChooseHeroReq(heroId).Forget();
        }

        private async UniTaskVoid ChooseHeroReq(int heroId)
        {
            var msg = ReferencePool.Acquire<CS_ChooseHeroReq>();
            msg.heroId = heroId;
            var ack = await SendPacketAsync<SC_ChooseHeroAck>(msg);
            if (ack.heroId > 0)
            {
                _currentHero.Visible = true;
                _currentHero.SetHeroInfo(ack.heroId);
            }
        }
    }
}