using NUnit.Framework;
using Assert = ModestTree.Assert;

namespace Zenject.Tests.Injection
{
    [TestFixture]
    public class TestPostInjectParameters : ZenjectUnitTestFixture
    {
        private class Test0
        {
        }

        private class Test1
        {
        }

        private class Test2
        {
        }

        private class Test3
        {
            public bool HasInitialized;

            public Test0 test0;

            [Inject]
            public Test1 test1 = null;

            [Inject]
            public void Init(
                Test0 test0,
                [InjectOptional]
                Test2 test2)
            {
                Assert.That(!HasInitialized);
                Assert.IsNotNull(test1);
                Assert.IsNull(test2);
                Assert.IsNull(this.test0);
                this.test0 = test0;
                HasInitialized = true;
            }
        }

        [Test]
        public void Test()
        {
            Container.Bind<Test1>().AsSingle().NonLazy();
            Container.Bind<Test3>().AsSingle().NonLazy();

            Container.Bind<Test0>().AsSingle().NonLazy();

            Test3 test3 = Container.Resolve<Test3>();

            Assert.That(test3.HasInitialized);
            Assert.IsNotNull(test3.test0);
        }
    }
}
