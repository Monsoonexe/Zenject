using NUnit.Framework;
using Assert = ModestTree.Assert;

namespace Zenject.Tests.Bindings
{
    [TestFixture]
    public class TestFactoryFromSubContainerInstaller1 : ZenjectUnitTestFixture
    {
        [Test]
        public void TestSelf()
        {
            Container.BindFactory<string, Foo, Foo.Factory>()
                .FromSubContainerResolve().ByInstaller<FooInstaller>().NonLazy();

            Assert.IsEqual(Container.Resolve<Foo.Factory>().Create("asdf").Value, "asdf");
        }

        [Test]
        public void TestConcrete()
        {
            Container.BindFactory<string, IFoo, IFooFactory>()
                .To<Foo>().FromSubContainerResolve().ByInstaller<FooInstaller>().NonLazy();

            Assert.IsEqual(Container.Resolve<IFooFactory>().Create("asdf").Value, "asdf");
        }

        private class FooInstaller : Installer<string, FooInstaller>
        {
            private readonly string _value;

            public FooInstaller(string value)
            {
                _value = value;
            }

            public override void InstallBindings()
            {
                Container.Bind<Foo>().AsTransient().WithArgumentsExplicit(
                    InjectUtil.CreateArgListExplicit(_value));
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

