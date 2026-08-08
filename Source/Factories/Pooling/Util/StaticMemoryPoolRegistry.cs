using ModestTree;
using System;
using System.Collections.Generic;
#if !NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    internal interface IResettableStaticMemoryPool
    {
        void ResetPool();
    }

    public static class StaticMemoryPoolRegistry
    {
        public static event Action<IMemoryPool> PoolAdded = delegate { };
        public static event Action<IMemoryPool> PoolRemoved = delegate { };

        private static readonly List<IMemoryPool> _pools = new List<IMemoryPool>();

        public static IEnumerable<IMemoryPool> Pools
        {
            get { return _pools; }
        }

        public static void Add(IMemoryPool memoryPool)
        {
            _pools.Add(memoryPool);
            PoolAdded(memoryPool);
        }

        public static void Remove(IMemoryPool memoryPool)
        {
            _pools.RemoveWithConfirm(memoryPool);
            PoolRemoved(memoryPool);
        }

        public static void Reset()
        {
            for (int i = _pools.Count - 1; i >= 0; i--)
            {
                var pool = _pools[i] as IResettableStaticMemoryPool;

                if (pool == null)
                {
                    _pools.RemoveAt(i);
                    continue;
                }

                pool.ResetPool();
            }
        }

#if !NOT_UNITY3D
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticState()
        {
            Reset();
            PoolAdded = delegate { };
            PoolRemoved = delegate { };
        }
#endif
    }
}
