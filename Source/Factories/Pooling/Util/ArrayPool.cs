using System;
using System.Collections.Generic;

namespace Zenject
{
    public class ArrayPool<T> : StaticMemoryPoolBaseBase<T[]>
    {
        private readonly int _length;

        private static readonly Dictionary<int, ArrayPool<T>> _pools =
            new Dictionary<int, ArrayPool<T>>();

        public ArrayPool(int length)
            : base(OnDespawned)
        {
            _length = length;
        }

        private static void OnDespawned(T[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = default(T);
            }
        }

        public T[] Spawn()
        {
#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                return SpawnInternal();
            }
        }

        protected override T[] Alloc()
        {
            return new T[_length];
        }

        public static ArrayPool<T> GetPool(int length)
        {
            if (!_pools.TryGetValue(length, out ArrayPool<T> pool))
            {
                pool = new ArrayPool<T>(length);
                _pools.Add(length, pool);
            }

            return pool;
        }

        public static PooledArray Spawn(out T[] array, int length)
        {
            ArrayPool<T> pool = GetPool(length);
            return new PooledArray(pool, array = pool.Spawn());
        }

        public readonly struct PooledArray : IDisposable
        {
            private readonly ArrayPool<T> pool;
            private readonly T[] item;

            public PooledArray(ArrayPool<T> pool, T[] item)
            {
                this.pool = pool;
                this.item = item;
            }

            public void Dispose() => pool.Despawn(item);
        }
    }
}
