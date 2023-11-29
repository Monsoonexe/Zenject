
#if !(UNITY_WSA && ENABLE_DOTNET)

using NUnit.Framework;
using Assert = ModestTree.Assert;

namespace Zenject.Tests.Convention.Names
{
    [TestFixture]
    public class TestConventionNames : ZenjectUnitTestFixture
    {
        [Test]
        public void TestWithSuffix()
        {
            Container.Bind<IController>()
                .To(x => x.AllNonAbstractClasses().InNamespace("Zenject.Tests.Convention.Names").WithSuffix("Controller")).AsTransient();

            Assert.That(Container.Resolve<IController>() is FooController);
        }

        [Test]
        public void TestWithPrefix()
        {
            Container.Bind<IController>()
                .To(x => x.AllTypes().InNamespace("Zenject.Tests.Convention.Names").WithPrefix("Controller")).AsTransient();

            Assert.That(Container.Resolve<IController>() is ControllerBar);
        }

        [Test]
        public void TestMatchingRegex()
        {
            Container.Bind<IController>()
                .To(x => x.AllNonAbstractClasses().InNamespace("Zenject.Tests.Convention.Names").MatchingRegex("Controller$")).AsTransient();

            Assert.That(Container.Resolve<IController>() is FooController);
        }

        private interface IController
        {
        }

        private class FooController : IController
        {
        }

        private class ControllerBar : IController
        {
        }

        private class QuxControllerAsdf : IController
        {
        }

        private class IgnoredFooController
        {
        }

        private class ControllerBarIgnored
        {
        }
    }
}

#endif
