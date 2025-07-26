using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace Game.Hot
{
    public class BattleCardPoolObject : ObjectBase
    {
        private Transform _poolRoot;

        public static BattleCardPoolObject Create(BattleCardUIElement element, Transform poolRoot)
        {
            var r = ReferencePool.Acquire<BattleCardPoolObject>();
            r._poolRoot = poolRoot;
            r.Initialize(element);
            return r;
        }

        public override void Clear()
        {
            _poolRoot = null;
            base.Clear();
        }

        protected override void Release(bool isShutdown)
        {
            Object.Destroy(((BattleCardUIElement)Target).gameObject);
        }

        protected override void OnUnspawn()
        {
            ((BattleCardUIElement)Target).CachedTransform.SetParent(_poolRoot, false);
        }
    }

    public partial class BattleForm
    {
        [SerializeField]
        private BattleCardUIElement _battleCardUIElementAsset;

        private IObjectPool<BattleCardPoolObject> _battleCardPool;

        private void InitHandCard()
        {
            _battleCardPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<BattleCardPoolObject>();
            _battleCardPool.Capacity = 30;
            _battleCardPool.AutoReleaseInterval = 30f;
            _battleCardPool.ExpireTime = 30f;

            var b = SpawnBattleCard(HandCardRect);
            b.SetCard(_myPlayer.HandCards[0]);
        }


        private BattleCardUIElement SpawnBattleCard(Transform parent)
        {
            BattleCardUIElement e = null;
            var poolObject = _battleCardPool.Spawn();
            if (poolObject == null)
            {
                e = Instantiate(_battleCardUIElementAsset, parent);
                var r = BattleCardPoolObject.Create(e, PoolRect);
                _battleCardPool.Register(r, true);
            }
            else
            {
                e = (BattleCardUIElement)poolObject.Target;
                e.CachedTransform.SetParent(parent, false);
            }
            return AddUIElement<BattleCardUIElement>(e);
        }

        private void UnSpawnBattleCard(BattleCardUIElement element)
        {
            RemoveUIElement(element);
            var r = BattleCardPoolObject.Create(element, PoolRect);
            _battleCardPool.Unspawn(r);
        }
    }
}