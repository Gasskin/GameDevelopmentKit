using GameFramework.ObjectPool;

namespace Game.Hot
{
    public partial class ObjectPoolComponent : HotComponent
    {
        
        protected override void OnInitialize()
        {
            CreateGameObjectPool();
        }
    }
}