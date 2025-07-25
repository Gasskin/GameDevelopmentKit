using CodeBind;
using UnityEngine;

namespace Game.Hot
{
    public partial class ProgressBarElement : AUIElement
    {
        private long _endTimestampMs;
        private int _totalLengthMs;

        protected override void OnAdd()
        {
            InitBind(gameObject.GetComponent<CSCodeBindMono>());
        }

        protected override void OnRemove()
        {
            ClearBind();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            float remain = _endTimestampMs - HotEntry.ServerTime.NowTimeMs;
            var per = Mathf.Clamp(remain / _totalLengthMs, 0f, 1f);
            CountDownUXImage.fillAmount = per;
        }

        public void StartProgress(long endTimestampMs, int totalLengthMs)
        {
            Visible = true;
            _endTimestampMs = endTimestampMs;
            _totalLengthMs = totalLengthMs;
        }
    }
}