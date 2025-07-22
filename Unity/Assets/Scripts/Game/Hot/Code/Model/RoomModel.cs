using System.Collections.Generic;
using GameFramework;
using UnityGameFramework.Extension;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public class RoomModel
    {
        public int MyAccount { get; private set; }

        // --------- 房间信息 --------
        public EBattleStage BattleStage { get; private set; } = EBattleStage.None;
        public int PlayerCount => _roomPlayers.Count;

        public bool IsRoomOwner => _roomPlayers.Count >= 1 && _roomPlayers[0] == MyAccount;

        private List<int> _roomPlayers = new();

        // --------- 局内信息 --------
        public long EndTimestampMs { get; private set; }
        public int TotalTimeMs { get; private set; }

        private List<int> _canChooseHero = new();

    #region Player相关
        public void SetRoomPlayers(int myAccount, List<int> players)
        {
            MyAccount = myAccount;

            _roomPlayers.Clear();
            foreach (var player in players)
                _roomPlayers.Add(player);

            ChangeBattleStage(EBattleStage.InRoom);

            Log.Info($"{myAccount} 成功加入房间，房间人数：{_roomPlayers.Count}");
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

        public int GetPlayerByIndex(int idx)
        {
            if (idx >= _roomPlayers.Count || idx < 0)
            {
                Log.Error("索引超出范围，无法获取玩家。索引: {0}, 房间人数: {1}", idx, _roomPlayers.Count);
                return 0;
            }
            return _roomPlayers[idx];
        }
    #endregion

    #region 战斗流程相关
        public void ChangeBattleStage(EBattleStage stage)
        {
            BattleStage = stage;
            if (stage == EBattleStage.None)
            {
                _roomPlayers.Clear();
                _canChooseHero.Clear();
            }
        }

        public void BeginBattle(SC_BeginBattleNtf msg)
        {
            _canChooseHero.Clear();
            _canChooseHero.AddRange(msg.canChooseHero);
            EndTimestampMs = msg.endTimestampMs;
            TotalTimeMs = msg.totalTimeMs;
        }

        public void GetCanChooseHero(UGFList<int> result)
        {
            result.AddRange(_canChooseHero);
        }
    #endregion
    }
}