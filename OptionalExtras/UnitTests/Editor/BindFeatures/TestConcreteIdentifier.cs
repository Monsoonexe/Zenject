using NUnit.Framework;
using Assert = ModestTree.Assert;

namespace Zenject.Tests.BindFeatures
{
    [TestFixture]
    public class TestConcreteIdentifier : ZenjectUnitTestFixture
    {
        [Test]
        public void Test1()
        {
            Container.Bind<IFoo>().To<Foo>().AsCached().WithConcreteId("asdf");
            Container.Bind<IFoo>().To<Foo>().AsCached();

            Container.BindInstance("a").When(x => Equals(x.ConcreteIdentifier, "asdf") && x.ObjectType == typeof(Foo));
            Container.BindInstance("b").When(x => x.ConcreteIdentifier == null && x.ObjectType == typeof(Foo));

            System.Collections.Generic.List<IFoo> foos = Container.ResolveAll<IFoo>();

            Assert.IsEqual(foos[0].Value, "a");
            Assert.IsEqual(foos[1].Value, "b");
        }

        private interface IFoo
        {
            string Value
            {
                get;
            }
        }

        private class Foo : IFoo
        {
            public Foo(string data)
            {
                Value = data;
            }

            public string Value
            {
                get; private set;
            }
        }
    }
}


