using CodeBind;
using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Hot
{
    public partial class BattleLoadingForm : AHotUIForm
    {
        private float _randomWait;
        private float _remain;

        private bool _isLoadEnd;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            InitBind(gameObject.GetComponent<CSCodeBindMono>());
            
            // 随机等待10-20秒，这个只是用来做显示的，没有实际意义
            _randomWait = (float)Utility.Random.GetRandomDouble() * 10f + 10f;
            _remain = _randomWait;
            TipTMPText.text = "资源加载中...";
            
            LoadTask().Forget();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            ClearBind();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            _remain -= elapseSeconds;
            if (_remain < 2)
            {
            }
            else
            {
                ProgressUXImage.fillAmount = Mathf.Clamp(1f - _remain / _randomWait, 0f, 1f);
            }
            //
            // if (_isLoadEnd && HotEntry.Model.RoomBattle.LoadingStage == EBattleStage.Loading)
            // {
            //     ProgressUXImage.fillAmount = 1f;
            //     HotEntry.Model.RoomBattle.ChangeRoomBattleLoadingStage(EBattleStage.LoadEnd);
            // }
        }

        private async UniTaskVoid LoadTask()
        {
            _isLoadEnd = false;
            await GameEntry.UI.OpenUIFormAsync(UIFormId.ChooseHeroForm, null, cancellationTokenSource.Token);
            _isLoadEnd = true;
        }
    }
}