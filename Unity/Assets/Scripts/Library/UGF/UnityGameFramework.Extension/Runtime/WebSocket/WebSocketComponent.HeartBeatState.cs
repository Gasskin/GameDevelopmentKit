namespace UnityGameFramework.Extension
{
    public sealed partial class WebSocketComponent
    {
        private sealed class HeartBeatState
        {
            private float m_HeartBeatElapseSeconds;
            private int m_MissHeartBeatCount;

            public HeartBeatState()
            {
                m_HeartBeatElapseSeconds = 0f;
                m_MissHeartBeatCount = 0;
            }

            public float HeartBeatElapseSeconds
            {
                get
                {
                    return m_HeartBeatElapseSeconds;
                }
                set
                {
                    m_HeartBeatElapseSeconds = value;
                }
            }

            public int MissHeartBeatCount
            {
                get
                {
                    return m_MissHeartBeatCount;
                }
                set
                {
                    m_MissHeartBeatCount = value;
                }
            }

            public void Reset(bool resetHeartBeatElapseSeconds)
            {
                if (resetHeartBeatElapseSeconds)
                {
                    m_HeartBeatElapseSeconds = 0f;
                }

                m_MissHeartBeatCount = 0;
            }
        }
    }
}