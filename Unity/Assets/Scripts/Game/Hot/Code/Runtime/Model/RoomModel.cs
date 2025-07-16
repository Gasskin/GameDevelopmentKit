using System.Collections.Generic;
using GameFramework;
using UnityGameFramework.Extension;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public class RoomModel
    {
        public int PlayerCount => _roomPlayers.Count;
        
        private List<int> _roomPlayers = new();

        public void SetRoomPlayers(List<int> players)
        {
            _roomPlayers.Clear();
            _roomPlayers.AddRange(players);

            Log.Info($"成功加入房间，房间人数：{_roomPlayers.Count}");
            for (int i = 0; i < _roomPlayers.Count; i++)
            {
                Log.Info($"玩家[{i}]：{_roomPlayers[i]}");
            }
            
            GameEntry.Event.Fire(this, ReferencePool.Acquire<RoomPlayerChangeEvent>());
        }

        public void AddRoomPlayer(int newPlayerId, List<int> players)
        {
            if (_roomPlayers.Contains(newPlayerId))
            {
                // 如果玩家已经在房间中，则不添加
                Log.Error("玩家已经在房间中，无法重复添加。玩家ID: {0}", newPlayerId);
                return;
            }
            _roomPlayers.Add(newPlayerId);

            Log.Info($"新玩家{newPlayerId}加入房间，房间人数：{_roomPlayers.Count}");
            for (int i = 0; i < _roomPlayers.Count; i++)
            {
                Log.Info($"玩家[{i}]：{_roomPlayers[i]}");
            }

            GameEntry.Event.Fire(this, ReferencePool.Acquire<RoomPlayerChangeEvent>());
        }

        public void RemoveRoomPlayer(int leavePlayer)
        {
            if (!_roomPlayers.Remove(leavePlayer))
            {
                // 如果玩家不在房间中，则无法移除
                Log.Error("玩家不在房间中，无法移除。玩家ID: {0}", leavePlayer);
                return;
            }
            Log.Info($"玩家{leavePlayer}离开房间，房间人数：{_roomPlayers.Count}");
            for (int i = 0; i < _roomPlayers.Count; i++)
            {
                Log.Info($"玩家[{i}]：{_roomPlayers[i]}");
            }
            
            GameEntry.Event.Fire(this, ReferencePool.Acquire<RoomPlayerChangeEvent>());
        }

        public void GetRoomPlayer(UGFList<int> players)
        {
            players.AddRange(_roomPlayers);
        }
    }
}