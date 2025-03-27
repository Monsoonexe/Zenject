using System;
using System.Collections.Generic;
using System.Text;

namespace Zenject.Internal
{
    public static class ZenPools
    {
#if ZEN_INTERNAL_NO_POOLS
        public static InjectContext SpawnInjectContext(DiContainer container, Type memberType)
        {
            return new InjectContext(container, memberType);
        }

        public static void DespawnInjectContext(InjectContext context)
        {
        }

        public static List<T> SpawnList<T>()
        {
            return new List<T>();
        }

        public static void DespawnList<T>(List<T> list)
        {
        }

        public static void DespawnArray<T>(T[] arr)
        {
        }

        public static T[] SpawnArray<T>(int length)
        {
            return new T[length];
        }

        public static HashSet<T> SpawnHashSet<T>()
        {
            return new HashSet<T>();
        }

        public static Dictionary<TKey, TValue> SpawnDictionary<TKey, TValue>()
        {
            return new Dictionary<TKey, TValue>();
        }

        public static void DespawnDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
        }

        public static void DespawnHashSet<T>(HashSet<T> set)
        {
        }

        public static LookupId SpawnLookupId(IProvider provider, BindingId bindingId)
        {
            return new LookupId(provider, bindingId);
        }

        public static void DespawnLookupId(LookupId lookupId)
        {
        }

        public static BindInfo SpawnBindInfo()
        {
            return new BindInfo();
        }

        public static void DespawnBindInfo(BindInfo bindInfo)
        {
        }

        public static BindStatement SpawnStatement()
        {
            return new BindStatement();
        }

        public static void DespawnStatement(BindStatement statement)
        {
        }

        public static PooledObject<T> SpawnList<T>(out List<T> list)
        {
            list = SpawnList();
            return default;
        }
        
        public readonly struct PooledObject<T> : IDisposable
        {
            public void Dispose() {}
        }
#else
        private static readonly StaticMemoryPool<InjectContext> _contextPool = new StaticMemoryPool<InjectContext>();
        private static readonly StaticMemoryPool<LookupId> _lookupIdPool = new StaticMemoryPool<LookupId>();
        private static readonly StaticMemoryPool<BindInfo> _bindInfoPool = new StaticMemoryPool<BindInfo>();
        private static readonly StaticMemoryPool<BindStatement> _bindStatementPool = new StaticMemoryPool<BindStatement>();
        public static readonly StaticMemoryPool<StringBuilder> StringBuilder = new StaticMemoryPool<StringBuilder>(
            onDespawnedMethod: (item) => item.Clear());

        public static PooledItem Spawn(out StringBuilder stringBuilder)
        {
            return new PooledItem(StringBuilder, stringBuilder = StringBuilder.Spawn());
        }

        public static HashSet<T> SpawnHashSet<T>()
        {
            return HashSetPool<T>.Instance.Spawn();
        }

        public static HashSetPool<T>.PooledItem Spawn<T>(out HashSet<T> set)
        {
            return HashSetPool<T>.Instance.Spawn(out set);
        }

        public static void DespawnHashSet<T>(HashSet<T> set)
        {
            HashSetPool<T>.Instance.Despawn(set);
        }

        public static Dictionary<TKey, TValue> SpawnDictionary<TKey, TValue>()
        {
            return DictionaryPool<TKey, TValue>.Instance.Spawn();
        }

        public static DictionaryPool<TKey, TValue>.PooledItem Spawn<TKey, TValue>(out Dictionary<TKey, TValue> dictionary)
        {
            return DictionaryPool<TKey, TValue>.Instance.Spawn(out dictionary);
        }

        public static void DespawnDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            DictionaryPool<TKey, TValue>.Instance.Despawn(dictionary);
        }

        public static BindStatement SpawnStatement()
        {
            return _bindStatementPool.Spawn();
        }

        public static void DespawnStatement(BindStatement statement)
        {
            statement.Reset();
            _bindStatementPool.Despawn(statement);
        }

        public static BindInfo SpawnBindInfo()
        {
            return _bindInfoPool.Spawn();
        }

        public static void DespawnBindInfo(BindInfo bindInfo)
        {
            bindInfo.Reset();
            _bindInfoPool.Despawn(bindInfo);
        }

        public static LookupId SpawnLookupId(IProvider provider, BindingId bindingId)
        {
            LookupId lookupId = _lookupIdPool.Spawn();

            lookupId.Provider = provider;
            lookupId.BindingId = bindingId;

            return lookupId;
        }

        public static void DespawnLookupId(LookupId lookupId)
        {
            lookupId.Reset();
            _lookupIdPool.Despawn(lookupId);
        }

        public static List<T> SpawnList<T>()
        {
            return ListPool<T>.Instance.Spawn();
        }

        // for use with 'using' statements
        public static ListPool<T>.PooledList Spawn<T>(out List<T> list)
        {
            return ListPool<T>.Instance.Spawn(out list);
        }

        public static void DespawnList<T>(List<T> list)
        {
            ListPool<T>.Instance.Despawn(list);
        }

        public static void DespawnArray<T>(T[] arr)
        {
            ArrayPool<T>.GetPool(arr.Length).Despawn(arr);
        }

        public static T[] SpawnArray<T>(int length)
        {
            return ArrayPool<T>.GetPool(length).Spawn();
        }

        // for use with 'using' statements
        public static ArrayPool<T>.PooledArray Spawn<T>(out T[] array, int length)
        {
            return ArrayPool<T>.Spawn(out array, length);
        }

        public static InjectContext SpawnInjectContext(DiContainer container, Type memberType)
        {
            InjectContext context = _contextPool.Spawn();

            context.Container = container;
            context.MemberType = memberType;

            return context;
        }

        public static void DespawnInjectContext(InjectContext context)
        {
            context.Reset();
            _contextPool.Despawn(context);
        }

#endif

        public static InjectContext SpawnInjectContext(
            DiContainer container, InjectableInfo injectableInfo, InjectContext currentContext,
            object targetInstance, Type targetType, object concreteIdentifier)
        {
            InjectContext context = SpawnInjectContext(container, injectableInfo.MemberType);

            context.ObjectType = targetType;
            context.ParentContext = currentContext;
            context.ObjectInstance = targetInstance;
            context.Identifier = injectableInfo.Identifier;
            context.MemberName = injectableInfo.MemberName;
            context.Optional = injectableInfo.Optional;
            context.SourceType = injectableInfo.SourceType;
            context.FallBackValue = injectableInfo.DefaultValue;
            context.ConcreteIdentifier = concreteIdentifier;

            return context;
        }


        public readonly struct PooledItem : IDisposable
        {
            private readonly StaticMemoryPool<StringBuilder> pool;
            private readonly StringBuilder item;

            public PooledItem(StaticMemoryPool<StringBuilder> pool, StringBuilder item)
            {
                this.pool = pool;
                this.item = item;
            }

            public void Dispose() => pool.Despawn(item);
        }
    }
}
