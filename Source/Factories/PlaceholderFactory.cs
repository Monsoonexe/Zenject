using System;
using System.Collections.Generic;
#if !NOT_UNITY3D
using JetBrains.Annotations;
using Zenject.Internal;
#endif

namespace Zenject
{
    // Zero parameters
    public class PlaceholderFactory<TValue> : PlaceholderFactoryBase<TValue>, IFactory<TValue>
    {
        // Note: Most of the time you should not override this method and should instead
        // use BindFactory<>.FromIFactory if you want to do some custom logic
#if !NOT_UNITY3D
        [NotNull]
#endif
        public virtual TValue Create()
        {
            using (ZenPools.Spawn(out List<TypeValuePair> args))
            {
                return CreateInternal(args);
            }
        }

        protected sealed override IEnumerable<Type> ParamTypes
        {
            get { yield break; }
        }
    }

    // One parameter
    public class PlaceholderFactory<TParam1, TValue>
        : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TValue>
    {
        // Note: Most of the time you should not override this method and should instead
        // use BindFactory<>.FromIFactory if you want to do some custom logic
#if !NOT_UNITY3D
        [NotNull]
#endif
        public virtual TValue Create(TParam1 param)
        {
            using (ZenPools.Spawn(out List<TypeValuePair> args))
            {
                args.Add(InjectUtil.CreateTypePair(param));
                return CreateInternal(args);
            }
        }

        protected sealed override IEnumerable<Type> ParamTypes
        {
            get { yield return typeof(TParam1); }
        }
    }

    // Two parameters
    public class PlaceholderFactory<TParam1, TParam2, TValue>
        : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TValue>
    {
        // Note: Most of the time you should not override this method and should instead
        // use BindFactory<>.FromIFactory if you want to do some custom logic
#if !NOT_UNITY3D
        [NotNull]
#endif
        public virtual TValue Create(TParam1 param1, TParam2 param2)
        {
            using (ZenPools.Spawn(out List<TypeValuePair> args))
            {
                args.Add(InjectUtil.CreateTypePair(param1));
                args.Add(InjectUtil.CreateTypePair(param2));
                return CreateInternal(args);
            }
        }

        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
            }
        }
    }


    // Three parameters
    public class PlaceholderFactory<TParam1, TParam2, TParam3, TValue>
        : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TValue>
    {
        // Note: Most of the time you should not override this method and should instead
        // use BindFactory<>.FromIFactory if you want to do some custom logic
#if !NOT_UNITY3D
        [NotNull]
#endif
        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            using (ZenPools.Spawn(out List<TypeValuePair> args))
            {
                args.Add(InjectUtil.CreateTypePair(param1));
                args.Add(InjectUtil.CreateTypePair(param2));
                args.Add(InjectUtil.CreateTypePair(param3));
                return CreateInternal(args);
            }
        }

        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
            }
        }
    }

    // Four parameters
    public class PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TValue>
        : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TValue>
    {
        // Note: Most of the time you should not override this method and should instead
        // use BindFactory<>.FromIFactory if you want to do some custom logic
#if !NOT_UNITY3D
        [NotNull]
#endif
        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            return CreateInternal(
                new List<TypeValuePair>
                {
                    InjectUtil.CreateTypePair(param1),
                    InjectUtil.CreateTypePair(param2),
                    InjectUtil.CreateTypePair(param3),
                    InjectUtil.CreateTypePair(param4)
                });
        }

        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
                yield return typeof(TParam4);
            }
        }
    }

    // Five parameters
    public class PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
        : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
    {
        // Note: Most of the time you should not override this method and should instead
        // use BindFactory<>.FromIFactory if you want to do some custom logic
#if !NOT_UNITY3D
        [NotNull]
#endif
        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            return CreateInternal(
                new List<TypeValuePair>
                {
                    InjectUtil.CreateTypePair(param1),
                    InjectUtil.CreateTypePair(param2),
                    InjectUtil.CreateTypePair(param3),
                    InjectUtil.CreateTypePair(param4),
                    InjectUtil.CreateTypePair(param5)
                });
        }

        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
                yield return typeof(TParam4);
                yield return typeof(TParam5);
            }
        }
    }

    // Six parameters
    public class PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>
        : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>
    {
        // Note: Most of the time you should not override this method and should instead
        // use BindFactory<>.FromIFactory if you want to do some custom logic
#if !NOT_UNITY3D
        [NotNull]
#endif
        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
        {
            return CreateInternal(
                new List<TypeValuePair>
                {
                    InjectUtil.CreateTypePair(param1),
                    InjectUtil.CreateTypePair(param2),
                    InjectUtil.CreateTypePair(param3),
                    InjectUtil.CreateTypePair(param4),
                    InjectUtil.CreateTypePair(param5),
                    InjectUtil.CreateTypePair(param6)
                });
        }

        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
                yield return typeof(TParam4);
                yield return typeof(TParam5);
                yield return typeof(TParam6);
            }
        }
    }

    // Ten parameters
    public class PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue>
        : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue>
    {
        // If you were hoping to override this method, use BindFactory<>.ToFactory instead
        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9, TParam10 param10)
        {
            return CreateInternal(
                new List<TypeValuePair>
                {
                    InjectUtil.CreateTypePair(param1),
                    InjectUtil.CreateTypePair(param2),
                    InjectUtil.CreateTypePair(param3),
                    InjectUtil.CreateTypePair(param4),
                    InjectUtil.CreateTypePair(param5),
                    InjectUtil.CreateTypePair(param6),
                    InjectUtil.CreateTypePair(param7),
                    InjectUtil.CreateTypePair(param8),
                    InjectUtil.CreateTypePair(param9),
                    InjectUtil.CreateTypePair(param10)
                });
        }

        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
                yield return typeof(TParam4);
                yield return typeof(TParam5);
                yield return typeof(TParam6);
                yield return typeof(TParam7);
                yield return typeof(TParam8);
                yield return typeof(TParam9);
                yield return typeof(TParam10);
            }
        }
    }
}

