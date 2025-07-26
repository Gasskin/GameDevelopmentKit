using GameFramework.ObjectPool;
using UnityEngine;

namespace Game.Hot
{
    public partial class ObjectPoolComponent
    {
        public IObjectPool<GameObjectInstance> GameObjectPool { get;private set; }

        private void CreateGameObjectPool()
        {
            GameObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<GameObjectInstance>();
            GameObjectPool.AutoReleaseInterval = 10f;
            GameObjectPool.ExpireTime = 30f;
            GameObjectPool.Capacity = 30;
        }

        public GameObject SpawnOrCreateGameObject(string assetName, Transform parent)
        {
            GameObject o = null;
            var poolObject = GameObjectPool.Spawn(assetName);
            if (poolObject != null)
            {
                o = (GameObject)poolObject.Target;
            }
            else
            {
                
            }
            return o;
        }
    }
}