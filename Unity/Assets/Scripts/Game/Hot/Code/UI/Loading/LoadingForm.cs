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

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitBind(gameObject.GetComponent<CSCodeBindMono>());

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccessEvent);
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccessEvent);
            ClearBind();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            _randomWait = (float)Utility.Random.GetRandomDouble() * 10f;
            _remain = _randomWait;
            TipTMPText.text = "资源加载中...";
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            _remain -= elapseSeconds;
            if (_remain < 0)
            {
                if (HotEntry.Model.RoomBattle.LoadingStage == EBattleStage.Loading)
                {
                    ProgressUXImage.fillAmount = 1f;
                    HotEntry.Model.RoomBattle.ChangeRoomBattleLoadingStage(EBattleStage.LoadEnd);
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