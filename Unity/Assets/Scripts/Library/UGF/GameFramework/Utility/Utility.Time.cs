using System;

namespace GameFramework
{
    public static partial class Utility
    {
        private static DateTime s_UtcZero = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static string FormatTime(long timeMs)
        {
            var t = s_UtcZero.AddMilliseconds(timeMs);
            return t.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}