namespace Game.Hot
{
    public class ModelComponent : HotComponent
    {
        public AccountModel Account { get; private set; }

        public RoomModel Room { get; private set; }

        public RoomBattleModel RoomBattle { get; private set; }
        
        protected override void OnInitialize()
        {
            Account = new AccountModel();
            Room = new RoomModel();
            RoomBattle = new RoomBattleModel();
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