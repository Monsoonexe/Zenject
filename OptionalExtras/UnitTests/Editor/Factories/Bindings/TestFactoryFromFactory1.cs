using NUnit.Framework;
using Assert = ModestTree.Assert;

namespace Zenject.Tests.Bindings
{
    [TestFixture]
    public class TestFactoryFromFactory1 : ZenjectUnitTestFixture
    {
        [Test]
        public void TestSelf()
        {
            Container.BindFactory<string, Foo, Foo.Factory>().FromIFactory(b => b.To<CustomFooFactory>().AsCached()).NonLazy();

            Assert.IsEqual(Container.Resolve<Foo.Factory>().Create("asdf").Value, "asdf");
        }

        [Test]
        public void TestConcrete()
        {
            Container.BindFactory<string, IFoo, IFooFactory>().To<Foo>().FromIFactory(b => b.To<CustomFooFactory>().AsCached()).NonLazy();

            Assert.IsEqual(Container.Resolve<IFooFactory>().Create("asdf").Value, "asdf");
        }

        private class CustomFooFactory : IFactory<string, Foo>
        {
            public Foo Create(string value)
            {
                return new Foo(value);
            }
        }

        private interface IFoo
        {
            string Value
            {
                get;
            }
        }

        private class IFooFactory : PlaceholderFactory<string, IFoo>
        {
        }

        private class Foo : IFoo
        {
            public Foo(string value)
            {
                Value = value;
            }

            public string Value
            {
                get;
                private set;
            }

            public class Factory : PlaceholderFactory<string, Foo>
            {
            }
        }
    }
}

