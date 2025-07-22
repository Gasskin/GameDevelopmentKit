using CodeBind;
using GameFramework;
using UnityEngine;

namespace Game.Hot
{
    public class ProgressBarWidgetOpenData : IReference
    {
        public long EndTimestampMs;

        public int TotalLengthMs;

        public void Clear()
        {
            EndTimestampMs = 0;
            TotalLengthMs = 0;
        }

        public static ProgressBarWidgetOpenData Create(long endTimestampMs, int totalLengthMs)
        {
            var r = ReferencePool.Acquire<ProgressBarWidgetOpenData>();
            r.EndTimestampMs = endTimestampMs;
            r.TotalLengthMs = totalLengthMs;
            return r;
        }
    }

    public partial class ProgressBarWidget : AUIWidget
    {
        private long _endTimestampMs;
        private int _totalLengthMs;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitBind(gameObject.GetComponent<CSCodeBindMono>());
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            var r = (ProgressBarWidgetOpenData)userData;
            _endTimestampMs = r.EndTimestampMs;
            _totalLengthMs = r.TotalLengthMs;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            float remain = _endTimestampMs - HotEntry.ServerTime.NowTimeMs;
            var per = Mathf.Clamp(remain / _totalLengthMs, 0f, 1f);
            CountDownUXImage.fillAmount = per;
        }
    }
}