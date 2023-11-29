
using NUnit.Framework;
using System;
using Assert = ModestTree.Assert;

namespace Zenject.Tests.Bindings
{
    [TestFixture]
    public class TestFromPoolableMemoryPoolZero : ZenjectUnitTestFixture
    {
        public class Foo : IPoolable<IMemoryPool>, IDisposable
        {
            private IMemoryPool _pool;

            public IMemoryPool Pool
            {
                get { return _pool; }
            }

            private void SetDefaults()
            {
                _pool = null;
            }

            public void Dispose()
            {
                _pool.Despawn(this);
            }

            public void OnDespawned()
            {
                _pool = null;
                SetDefaults();
            }

            public void OnSpawned(IMemoryPool pool)
            {
                _pool = pool;
            }

            public class Factory : PlaceholderFactory<Foo>
            {
            }
        }

        [Test]
        public void Test1()
        {
            Container.BindFactory<Foo, Foo.Factory>().FromPoolableMemoryPool(x => x.WithInitialSize(2).FromNew());

            Foo.Factory factory = Container.Resolve<Foo.Factory>();

            Foo foo = factory.Create();
            IMemoryPool pool = foo.Pool;

            Assert.IsEqual(pool.NumActive, 1);
            Assert.IsEqual(pool.NumTotal, 2);
            Assert.IsEqual(pool.NumInactive, 1);

            foo.Dispose();

            Assert.IsEqual(pool.NumActive, 0);
            Assert.IsEqual(pool.NumTotal, 2);
            Assert.IsEqual(pool.NumInactive, 2);
        }
    }
}