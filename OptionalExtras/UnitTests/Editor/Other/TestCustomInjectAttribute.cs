
using NUnit.Framework;
using System;
using Zenject.Internal;
using Assert = ModestTree.Assert;

namespace Zenject.Tests.Other
{
    [TestFixture]
    public class TestCustomInjectAttribute : ZenjectUnitTestFixture
    {
        public class InjectCustomAttribute : Attribute
        {
        }

        private class Bar
        {
        }

        [NoReflectionBaking]
        private class Foo
        {
            [InjectCustom]
            public Bar BarField = null;

            public Foo(Bar barParam)
            {
                BarParam = barParam;
            }

            public Bar BarParam;
            public Bar BarMethod;

            [InjectCustom]
            public Bar BarProperty
            {
                get; private set;
            }

            [InjectCustom]
            public void Construct(Bar bar)
            {
                BarMethod = bar;
            }
        }

        [Test]
        public void Test1()
        {
            ReflectionTypeAnalyzer.AddCustomInjectAttribute(typeof(InjectCustomAttribute));

            Container.Bind<Bar>().AsSingle();
            Container.Bind<Foo>().AsSingle();

            Foo foo = Container.Resolve<Foo>();
            Bar bar = Container.Resolve<Bar>();

            Assert.IsEqual(foo.BarProperty, bar);
            Assert.IsEqual(foo.BarField, bar);
            Assert.IsEqual(foo.BarMethod, bar);
            Assert.IsEqual(foo.BarParam, bar);
        }
    }
}
