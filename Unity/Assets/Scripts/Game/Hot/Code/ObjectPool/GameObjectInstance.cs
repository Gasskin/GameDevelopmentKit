using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace Game.Hot
{
    public class GameObjectInstance : ObjectBase
    {
        private object _asset;

        public GameObjectInstance Create(string name, GameObject asset, GameObject instance)
        {
            var r = ReferencePool.Acquire<GameObjectInstance>();
            r._asset = asset;
            r.Initialize(name, instance);
            return r;
        }

        public override void Clear()
        {
            base.Clear();
            _asset = null;
        }

        protected override void Release(bool isShutdown)
        {
            GameEntry.Resource.UnloadAsset(_asset);
            Object.Destroy((Object)Target);
        }
    }
}