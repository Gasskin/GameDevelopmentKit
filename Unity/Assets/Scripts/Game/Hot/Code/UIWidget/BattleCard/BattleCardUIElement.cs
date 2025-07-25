using System;
using System.Collections.Generic;
using CodeBind;

namespace Game.Hot
{
    public partial class BattleCardUIElement : AUIElement
    {
        public event EventHandler<int> ClickBattleCardEventHandler;

        private BattleCard _battleCard;

        protected override void OnAdd()
        {
            InitBind(CachedGameObject.GetComponent<CSCodeBindMono>());

            CardBgButton.onClick.AddListener(OnCardClick);
        }

        protected override void OnRemove()
        {
            _battleCard = null;
            ClickBattleCardEventHandler = null;
            CardBgButton.onClick.RemoveAllListeners();
            ClearBind();
        }

        public void SetCard(int cardId)
        {
            _battleCard = HotEntry.Model.RoomBattle.GetBattleCard(cardId);
            if (_battleCard != null && HotEntry.Tables.DTCardConfig.DataMap.TryGetValue(cardId, out var config))
            {
                CardNameTMPText.text = config.CardName;
                CardInfoTMPText.text = GetCardInfo();
            }
        }

        private void OnCardClick()
        {
            if (_battleCard != null) 
            {
                ClickBattleCardEventHandler?.Invoke(this, _battleCard.CardId);
            }
        }
        
        private string GetCardInfo()
        {
            switch (_battleCard.CardAttr)
            {
                case ECardAttr.Jin:
                    return "";
                case ECardAttr.Mu:
                    return "";
                case ECardAttr.Shui:
                    return "";
                case ECardAttr.Huo:
                    return "";
                case ECardAttr.Tu:
                    return "";
                default:
                    return "";
            }
        }
    }
}