using CodeBind;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Hot
{
    public partial class BattleForm : AHotUIForm
    {
        private RoomBattleModel _roomBattleModel;
        private RoomModel _roomModel;

        private HeroCardElement _myHeroCardElement;
        private HeroCardElement _enemyHeroCardElement;
        
        private BattlePlayer _myPlayer;
        private BattlePlayer _enemyPlayer;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            InitBind(gameObject.GetComponent<CSCodeBindMono>());
            _myHeroCardElement = AddUIElement<HeroCardElement>(MyHeroCardUIElement);
            _enemyHeroCardElement = AddUIElement<HeroCardElement>(EnemyHeroCardUIElement);

            _roomBattleModel = HotEntry.Model.RoomBattle;
            _roomModel = HotEntry.Model.Room;

            _myPlayer = _roomBattleModel.GetPlayerByIndex(0);
            _enemyPlayer= _roomBattleModel.GetPlayerByIndex(1);
            _myHeroCardElement.SetBattlePlayerInfo(_myPlayer);
            _enemyHeroCardElement.SetBattlePlayerInfo(_enemyPlayer);
            
            InitHandCard();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            ClearBind();
        }
    }
}