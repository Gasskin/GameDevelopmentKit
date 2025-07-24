using System.Collections.Generic;
using CodeBind;
using GameFramework;
using GameFramework.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Extension;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public partial class RoomForm : AHotUIForm
    {
        private RoomModel _roomModel;

        private List<TMP_Text> _playerName = new(2);
        private List<UXImage> _playerState = new(2);

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            InitBind(gameObject.GetComponent<CSCodeBindMono>());
            
            _playerName.Add(Player1TMPText);
            _playerName.Add(Player2TMPText);
            _playerState.Add(State1UXImage);
            _playerState.Add(State2UXImage);
            
            _roomModel = HotEntry.Model.Room;
            
            StartGameButton.onClick.AddListener(OnStartGameClick);
            
            GameEntry.Event.Subscribe(RoomPlayerChangeEvent.EventId, OnRoomPlayerChangeEvent);
            
            OnRoomPlayerChangeEvent(null, null);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            StartGameButton.onClick.RemoveAllListeners();
            ClearBind();
        }

        private void ResetSeat()
        {
            Player1TMPText.text = "等待加入";
            Player2TMPText.text = "等待加入";
            
            State1UXImage.color = Color.yellow;
            State2UXImage.color = Color.yellow;
            
            StartGameUXImage.SetGrey(true);
        }
        
        private void OnRoomPlayerChangeEvent(object sender, GameEventArgs e)
        {
            ResetSeat();
            using var list = UGFList<int>.Create();
            _roomModel.GetRoomPlayer(list);
            for (int i = 0; i < list.Count && i < 2; i++)
            {
                _playerName[i].text = list[i].ToString();
                _playerState[i].color = Color.green;
            }
            StartGameUXImage.SetGrey(_roomModel.PlayerCount < 2);
            StartGameButton.gameObject.SetActive(_roomModel.IsRoomOwner);
        }
        
        private void OnStartGameClick()
        {
            if (_roomModel.PlayerCount < 2)
            {
                return;
            }
            GameEntry.Network.SendTcp(ReferencePool.Acquire<CS_BeginBattleNtf>());
        }
    }
}