using NUnit.Framework;
using Assert = ModestTree.Assert;

namespace Zenject.Tests.Injection
{
    [TestFixture]
    public class TestBaseClassPropertyInjection : ZenjectUnitTestFixture
    {
        private class Test0
        {
        }

        private class Test3
        {
        }

        private class Test1 : Test3
        {
            [Inject] protected Test0 val = null;

            public Test0 GetVal()
            {
                return val;
            }
        }

        private class Test2 : Test1
        {
        }

        [Test]
        public void TestCaseBaseClassPropertyInjection()
        {
            Container.Bind<Test0>().AsSingle().NonLazy();
            Container.Bind<Test2>().AsSingle().NonLazy();

            Test2 test1 = Container.Resolve<Test2>();

            Assert.That(test1.GetVal() != null);
        }
    }
}


