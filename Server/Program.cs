using System.Diagnostics;
using cfg;

class Program
{
    public static long ServerTimeMs { get;private set; }
    
    private const double FRAME_TIME = 1000.0 / 30;
    private static bool _isRunning = true;
    
    static void Main()
    {
        MessageDispatcher.Instance.AutoRegisterHandlers(new GameMessageHandler());
        
        Task.Run(() => Server.Instance.Init());
        Tables.Instance.Init();
        Battle.Instance.Init();
        
        var stopwatch = new Stopwatch();
        while (_isRunning)
        {
            stopwatch.Restart();
            Update();
            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed.TotalMilliseconds;
            var waitTime = FRAME_TIME - elapsed;
            if (waitTime > 0)
                Thread.Sleep((int)waitTime);
        }
    }

    private static void Update()
    {
        ServerTimeMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        MessageDispatcher.Instance.Update(); 
        Battle.Instance.Update();
    }
}
