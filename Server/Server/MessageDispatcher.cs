using System.Net.Sockets;

public class MessageDispatcher
{
    public static MessageDispatcher Instance { get; } = new();

    private readonly Dictionary<int, Action<Socket, byte[]>> _handlers = new();
    private readonly Queue<Action> _postQueue = new();

    public void Register<T>(int msgId, Action<Socket, T> handler) where T : class
    {
        _handlers[msgId] = (client, body) =>
        {
            try
            {
                using var ms = new MemoryStream(body);
                var msg = ProtoBuf.Serializer.Deserialize<T>(ms);
                handler(client, msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[日志]处理消息失败 异常={ex.Message}\n{ex.StackTrace}");
            }
        };
    }

    public void AutoRegisterHandlers(GameMessageHandler handler)
    {
        handler.Register(this);
    }

    /// <summary>
    /// 子线程中调用，将消息调度到主线程处理
    /// </summary>
    public void Post(Socket client, int msgId, byte[] body)
    {
        lock (_postQueue)
        {
            _postQueue.Enqueue(() => Dispatch(client, msgId, body));
        }
    }

    /// <summary>
    /// 主线程每帧调用
    /// </summary>
    public void Update()
    {
        lock (_postQueue)
        {
            while (_postQueue.Count > 0)
            {
                var action = _postQueue.Dequeue();
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[日志]消息派发异常: {ex.Message}");
                }
            }
        }
    }

    /// <summary>
    /// 只能在主线程中调用
    /// </summary>
    private void Dispatch(Socket client, int msgId, byte[] body)
    {
        if (_handlers.TryGetValue(msgId, out var handler))
        {
            handler(client, body);
        }
        else
        {
            Console.WriteLine($"[日志]未注册的消息处理器: {msgId}");
        }
    }
}