using GameFramework;

namespace Game.Hot
{
    public class AccountModel 
    {
        public int AccountId { get; private set; }

        public void SetAccountId(int id)
        {
            AccountId = id;
        }
    }
}


