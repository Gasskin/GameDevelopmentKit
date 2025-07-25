using CodeBind;
using UnityEngine;

namespace Game.Hot
{
    public partial class BattleForm : AHotUIForm
    {
        private RoomBattleModel _roomBattleModel;
        private RoomModel _roomModel;
        
        private HeroCardElement _myHeroCardElement;
        private HeroCardElement _enemyHeroCardElement;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            InitBind(gameObject.GetComponent<CSCodeBindMono>());
            _myHeroCardElement = AddUIElement<HeroCardElement>(MyHeroCardUIElement);
            _enemyHeroCardElement = AddUIElement<HeroCardElement>(EnemyHeroCardUIElement);
            
            _roomBattleModel = HotEntry.Model.RoomBattle;
            _roomModel = HotEntry.Model.Room;
            
            _myHeroCardElement.SetBattlePlayerInfo(_roomBattleModel.GetPlayerByIndex(0));
            _enemyHeroCardElement.SetBattlePlayerInfo(_roomBattleModel.GetPlayerByIndex(1));
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            ClearBind();
        }
    }
}