using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Assert = ModestTree.Assert;

namespace Zenject.Tests.Injection
{
    [TestFixture]
    public class TestListInjection : ZenjectUnitTestFixture
    {
        [Test]
        public void TestConstructor1()
        {
            BindListItems();
            Container.Bind<Test1>().AsSingle();
            TestListItems(Container.Resolve<Test1>().Values);
        }

        [Test]
        public void TestField1()
        {
            BindListItems();
            Container.Bind<Test3>().AsSingle();
            TestListItems(Container.Resolve<Test3>().Values);
        }

        [Test]
        public void TestIList()
        {
            BindListItems();
            Container.Bind<Test2>().AsSingle();
            TestListItems(Container.Resolve<Test2>().Values.ToList());
        }

        [Test]
        public void TestIEnumerable()
        {
            BindListItems();
            Container.Bind<Test4>().AsSingle();
            TestListItems(Container.Resolve<Test4>().Values.ToList());
        }

        [Test]
        public void TestArrays()
        {
            BindListItems();
            Container.Bind<Test5>().AsSingle();
            TestListItems(Container.Resolve<Test5>().Values.ToList());
        }

        private void BindListItems()
        {
            Container.BindInstance("foo");
            Container.BindInstance("bar");
        }

        private void TestListItems(List<string> values)
        {
            Assert.IsEqual(values[0], "foo");
            Assert.IsEqual(values[1], "bar");
        }

        private class Test1
        {
            public Test1(List<string> values)
            {
                Values = values;
            }

            public List<string> Values
            {
                get; private set;
            }
        }

        private class Test3
        {
            [Inject]
            public List<string> Values = null;
        }

        private class Test2
        {
            public Test2(IList<string> values)
            {
                Values = values;
            }

            public IList<string> Values
            {
                get; private set;
            }
        }

        private class Test4
        {
            public Test4(IEnumerable<string> values)
            {
                Values = values;
            }

            public IEnumerable<string> Values
            {
                get; private set;
            }
        }

        private class Test5
        {
            public Test5(string[] values)
            {
                Values = values;
            }

            public string[] Values
            {
                get; private set;
            }
        }
    }
}
