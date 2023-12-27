using ModestTree;
using System;
using System.Collections.Generic;

namespace Zenject
{
    public class DictionaryPool<TKey, TValue> : StaticMemoryPool<Dictionary<TKey, TValue>>
    {
        private static readonly DictionaryPool<TKey, TValue> _instance = new DictionaryPool<TKey, TValue>();

        public DictionaryPool()
        {
#if !ZEN_STRIP_ASSERTS_IN_BUILDS
            OnSpawnMethod = OnSpawned;
#endif
            OnDespawnedMethod = OnDespawned;
        }

        public static DictionaryPool<TKey, TValue> Instance
        {
            get { return _instance; }
        }

        public PooledItem Spawn(out Dictionary<TKey, TValue> list)
        {
            return new PooledItem(this, list = Spawn());
        }

#if UNITY_EDITOR
        // Required for disabling domain reload in enter the play mode feature. See: https://docs.unity3d.com/Manual/DomainReloading.html
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticValues()
        {
            if (!UnityEditor.EditorSettings.enterPlayModeOptionsEnabled)
            {
                return;
            }

            _instance.Clear();
        }
#endif

        private static void OnSpawned(Dictionary<TKey, TValue> items)
        {
            Assert.That(items.IsEmpty());
        }

        private static void OnDespawned(Dictionary<TKey, TValue> items)
        {
            items.Clear();
        }

        public readonly struct PooledItem : IDisposable
        {
            private readonly DictionaryPool<TKey, TValue> pool;
            private readonly Dictionary<TKey, TValue> item;

            public PooledItem(DictionaryPool<TKey, TValue> pool, Dictionary<TKey, TValue> item)
            {
                this.pool = pool;
                this.item = item;
            }

            public void Dispose() => pool.Despawn(item);
        }
    }
}
