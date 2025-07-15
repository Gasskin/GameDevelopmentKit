namespace Game.Hot
{
    public class ModelComponent : HotComponent
    {
        public AccountModel Account { get; private set; }
        
        protected override void OnInitialize()
        {
            Account = new AccountModel();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }

        protected override void OnShutdown()
        {
            Account = null;
        }
    }
}