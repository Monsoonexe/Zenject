using System;
using System.Collections.Generic;

namespace Zenject
{
    public class ListPool<T> : StaticMemoryPool<List<T>>
    {
        private static readonly ListPool<T> _instance = new ListPool<T>();

        public ListPool()
        {
            OnDespawnedMethod = OnDespawned;
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

        public static ListPool<T> Instance => _instance;

        private static void OnDespawned(List<T> list)
        {
            list.Clear();
        }

        public PooledList Spawn(out List<T> list)
        {
            return new PooledList(this, list = Spawn());
        }

        public readonly struct PooledList : IDisposable
        {
            private readonly ListPool<T> pool;
            private readonly List<T> item;

            public PooledList(ListPool<T> pool, List<T> item)
            {
                this.pool = pool;
                this.item = item;
            }

            public void Dispose() => pool.Despawn(item);
        }
    }
}
