using System.Diagnostics;

class Program
{
    private const double FRAME_TIME = 1000.0 / 30;
    private static bool _isRunning = true;
    
    static void Main()
    {
        MessageDispatcher.Instance.AutoRegisterHandlers(new GameMessageHandler());
        
        Task.Run(() => Server.Instance.Init());
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
        MessageDispatcher.Instance.Update(); 
        Battle.Instance.Update();
    }
}
