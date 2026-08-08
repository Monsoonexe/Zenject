using System;
using System.Collections.Generic;
#if !NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    public class ListPool<T> : StaticMemoryPool<List<T>>
    {
        private static readonly ListPool<T> _instance = new ListPool<T>();

        public ListPool()
        {
            OnDespawnedMethod = OnDespawned;
        }

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

#if !NOT_UNITY3D
    internal static class ListPoolRuntimeState
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticState()
        {
            StaticMemoryPoolRegistry.Reset();
        }
    }
#endif
}
