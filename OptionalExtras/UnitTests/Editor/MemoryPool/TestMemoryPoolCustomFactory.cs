using NUnit.Framework;
using Assert = ModestTree.Assert;

#pragma warning disable 219

namespace Zenject.Tests.Bindings
{
    [TestFixture]
    public class TestMemoryPoolCustomFactory : ZenjectUnitTestFixture
    {
        [Test]
        public void TestFromBinding()
        {
            Container.BindMemoryPool<Qux, Qux.Pool>().FromIFactory(b => b.To<CustomFactory>().AsCached());

            Qux.Pool pool = Container.Resolve<Qux.Pool>();

            Qux qux = pool.Spawn();

            Assert.IsEqual(pool.NumTotal, 1);
        }

        [Test]
        public void TestFromRuntime()
        {
            var settings = new MemoryPoolSettings(0, int.MaxValue, PoolExpandMethods.OneAtATime);

            Qux.Pool pool = Container.Instantiate<Qux.Pool>(new object[] { settings, new CustomFactory() });

            Qux qux = pool.Spawn();

            Assert.IsEqual(pool.NumTotal, 1);
        }

        private class CustomFactory : IFactory<Qux>
        {
            public Qux Create()
            {
                return new Qux();
            }
        }

        private class Qux
        {
            public class Pool : MemoryPool<Qux>
            {
            }
        }
    }
}

