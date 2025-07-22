using Game.Hot;

public class WaitForClientReady
{
    private List<int> _waitForClient;
    
    public WaitForClientReady(List<int> players)
    {
        _waitForClient = new List<int>(players);
    }

    public bool Wait()
    {
        return _waitForClient.Count <= 0;
    }
}