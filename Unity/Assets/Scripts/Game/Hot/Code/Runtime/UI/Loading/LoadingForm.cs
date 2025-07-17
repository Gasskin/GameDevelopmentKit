using CodeBind;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public partial class LoadingForm : AHotUIForm
    {
        private float _randomWait;
        private float _remain;

        private VarInt32 _battleStage;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitBind(gameObject.GetComponent<CSCodeBindMono>());

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccessEvent);
            _battleStage = GameEntry.DataNode.GetData<VarInt32>(HotConstant.DataNode.BATTLE_STAGE);
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccessEvent);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            _randomWait = Utility.Random.GetRandom(5, 15);
            _remain = _randomWait;
            TipTMPText.text = "资源加载中...";
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            _remain -= elapseSeconds;

            if (_remain < 0)
            {
                if (_battleStage.Value == (int)BattleStage.LoadGameAsset)
                {
                    _battleStage.Value = (int)BattleStage.ReadyForGame;
                    ProgressUXImage.fillAmount = 1f;
                    TipTMPText.text = "等待其他玩家...";
                }
            }
            else
            {
                ProgressUXImage.fillAmount = Mathf.Clamp(1f - _remain / _randomWait, 0f, 1f);
            }
        }

        private void OnOpenUIFormSuccessEvent(object sender, GameEventArgs e)
        {
        }
    }
}