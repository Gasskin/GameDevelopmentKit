using System;
using CodeBind;

namespace Game.Hot
{
    public partial class HeroCardElement : AUIElement
    {
        private bool _canClick;
        private Action<int> _onClick;
        private int _heroId;

        protected override void OnOpen()
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

        protected override void OnClose()
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

        public void SetCanClick(Action<int> onClick)
        {
            _onClick = onClick;
            _canClick = _onClick != null;
        }
    }
}