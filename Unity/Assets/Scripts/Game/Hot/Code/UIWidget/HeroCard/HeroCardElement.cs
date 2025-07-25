using System;
using CodeBind;
using GameFramework;

namespace Game.Hot
{
    public partial class HeroCardElement : AUIElement
    {
        private bool _canClick;
        private Action<int> _onClick;
        private int _heroId;
        private BattlePlayer _battlePlayer;

        protected override void OnAdd()
        {
            InitBind(gameObject.GetComponent<CSCodeBindMono>());
            HeroButton.onClick.AddListener((() =>
            {
                if (_canClick && _heroId > 0)  
                {
                    _onClick?.Invoke(_heroId);
                }
            }));
        }

        protected override void OnRemove()
        {
            _heroId = 0;
            ClearBind();
        }

        public void SetHeroInfo(int heroId)
        {
            var drHero = HotEntry.Tables.DTHero.GetOrDefault(heroId);
            if (drHero != null)
            {
                _heroId = heroId;
                HeroNameTMPText.text = drHero.Name;
                HandCardTMPText.text = drHero.HandLimit.ToString();
                HpTMPText.text = drHero.HpLimit.ToString();
            }
        }

        public void SetBattlePlayerInfo(BattlePlayer p)
        {
            var drHero = HotEntry.Tables.DTHero.GetOrDefault(p.HeroId);
            if (drHero != null)
            {
                _heroId = p.HeroId;
                _battlePlayer = p;
                HeroNameTMPText.text = drHero.Name;
                HandCardTMPText.text = Utility.Text.Format("{0}/{1}", _battlePlayer.HandCards.Count, _battlePlayer.BattleProperty[EPropertyType.HandLimit]);
                HpTMPText.text = Utility.Text.Format("{0}/{1}", _battlePlayer.BattleProperty[EPropertyType.CurHp], _battlePlayer.BattleProperty[EPropertyType.MaxHp]);
            }
        }

        public void SetCanClick(Action<int> onClick)
        {
            _onClick = onClick;
            _canClick = _onClick != null;
        }
    }
}