using NUnit.Framework;
using Assert = ModestTree.Assert;

namespace Zenject.Tests.Conditions
{
    [TestFixture]
    public class TestConditionsTarget : ZenjectUnitTestFixture
    {
        private class Test0
        {
        }

        private class Test1
        {
            public Test1(Test0 test)
            {
            }
        }

        private class Test2
        {
            public Test2(Test0 test)
            {
            }
        }

        public override void Setup()
        {
            base.Setup();
            Container.Bind<Test0>().AsSingle().When(r => r.ObjectType == typeof(Test2));
        }

        [Test]
        public void TestTargetConditionError()
        {
            Container.Bind<Test1>().AsSingle().NonLazy();

            Assert.Throws(
                delegate
                { Container.Resolve<Test1>(); });
        }

        [Test]
        public void TestTargetConditionSuccess()
        {
            Container.Bind<Test2>().AsSingle().NonLazy();

            Test2 test2 = Container.Resolve<Test2>();

            Assert.That(test2 != null);
        }
    }
}


