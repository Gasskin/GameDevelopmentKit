using GameFramework;

namespace Game.Hot
{
    public class ServerTimeComponent : HotComponent
    {
        public long NowTimeMs { get; private set; }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (NowTimeMs > 0)
            {
                NowTimeMs += (int)(elapseSeconds * 1000);
            }
        }
        
        public void Sync(long nowTime)
        {
            NowTimeMs = nowTime;
        }
    }
}