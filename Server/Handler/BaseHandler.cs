namespace Game.Hot.Handler;

public abstract class BaseHandler
{
    public abstract int Id { get; }

    public abstract void Handle();
}