
using ModestTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace Zenject.Tests.Bindings
{
    public class TestFromComponentInParents : ZenjectIntegrationTestFixture
    {
        private Root _root;
        private Child _child1;
        private Child _child2;
        private Child _child3;
        private Grandchild _grandchild;

        public void Setup1()
        {
            _root = new GameObject().AddComponent<Root>();

            _child1 = new GameObject().AddComponent<Child>();
            _child1.transform.SetParent(_root.transform);

            _child2 = new GameObject().AddComponent<Child>();
            _child2.transform.SetParent(_child1.transform);

            _child3 = _child2.gameObject.AddComponent<Child>();

            _grandchild = new GameObject().AddComponent<Grandchild>();
            _grandchild.transform.SetParent(_child2.transform);
        }

        public void Setup2()
        {
            _root = new GameObject().AddComponent<Root>();

            _grandchild = new GameObject().AddComponent<Grandchild>();
            _grandchild.transform.SetParent(_root.transform);
        }

        [UnityTest]
        public IEnumerator RunMatchSingleParent()
        {
            Setup1();
            PreInstall();
            Container.Bind<Root>().FromComponentInParents();
            Container.Bind<Child>().FromComponentInParents();

            PostInstall();

            Assert.IsEqual(_grandchild.Childs.Count, 1);
            Assert.IsEqual(_grandchild.Childs[0], _child2);
            Assert.IsEqual(_grandchild.Root, _root);
            yield break;
        }

        [UnityTest]
        public IEnumerator RunMatchMultipleParents()
        {
            Setup1();
            PreInstall();
            Container.Bind<Root>().FromComponentInParents();
            Container.Bind<Child>().FromComponentsInParents();

            PostInstall();

            Assert.IsEqual(_grandchild.Childs.Count, 3);
            Assert.IsEqual(_grandchild.Childs[0], _child2);
            Assert.IsEqual(_grandchild.Childs[1], _child3);
            Assert.IsEqual(_grandchild.Childs[2], _child1);
            Assert.IsEqual(_grandchild.Root, _root);
            yield break;
        }

        [UnityTest]
        public IEnumerator RunMissingParentFailure()
        {
            Root root = new GameObject().AddComponent<Root>();

            Grandchild2 grandchild = new GameObject().AddComponent<Grandchild2>();
            grandchild.transform.SetParent(root.transform);

            PreInstall();
            Container.Bind<Root>().FromComponentInParents();
            Container.Bind<Child>().FromComponentInParents();

            Assert.Throws(() => PostInstall());
            yield break;
        }

        [UnityTest]
        public IEnumerator RunMissingParentSuccess()
        {
            Setup2();
            PreInstall();
            Container.Bind<Root>().FromComponentInParents();
            Container.Bind<Child>().FromComponentsInParents();

            PostInstall();

            Assert.IsEqual(_grandchild.Childs.Count, 0);
            Assert.IsEqual(_grandchild.Root, _root);
            yield break;
        }

        [UnityTest]
        public IEnumerator TestOptional()
        {
            new GameObject();
            ChildWithOptional child = new GameObject().AddComponent<ChildWithOptional>();

            PreInstall();

            Container.Bind<Root>().FromComponentInParents();

            PostInstall();

            Assert.IsNull(child.Root);
            yield break;
        }

        [UnityTest]
        public IEnumerator RunMatchSingleParentNonGeneric()
        {
            Setup1();
            PreInstall();
            Container.Bind(typeof(Root)).FromComponentInParents();
            Container.Bind(typeof(Child)).FromComponentInParents();

            PostInstall();

            Assert.IsEqual(_grandchild.Childs.Count, 1);
            Assert.IsEqual(_grandchild.Childs[0], _child2);
            Assert.IsEqual(_grandchild.Root, _root);
            yield break;
        }

        [UnityTest]
        public IEnumerator RunMatchMultipleParentsNonGeneric()
        {
            Setup1();
            PreInstall();
            Container.Bind(typeof(Root)).FromComponentInParents();
            Container.Bind(typeof(Child)).FromComponentsInParents();

            PostInstall();

            Assert.IsEqual(_grandchild.Childs.Count, 3);
            Assert.IsEqual(_grandchild.Childs[0], _child2);
            Assert.IsEqual(_grandchild.Childs[1], _child3);
            Assert.IsEqual(_grandchild.Childs[2], _child1);
            Assert.IsEqual(_grandchild.Root, _root);
            yield break;
        }

        [UnityTest]
        public IEnumerator RunMissingParentFailureNonGeneric()
        {
            Root root = new GameObject().AddComponent<Root>();

            Grandchild2 grandchild = new GameObject().AddComponent<Grandchild2>();
            grandchild.transform.SetParent(root.transform);

            PreInstall();
            Container.Bind(typeof(Root)).FromComponentInParents();
            Container.Bind(typeof(Child)).FromComponentInParents();

            Assert.Throws(() => PostInstall());
            yield break;
        }

        [UnityTest]
        public IEnumerator RunMissingParentSuccessNonGeneric()
        {
            Setup2();
            PreInstall();
            Container.Bind(typeof(Root)).FromComponentInParents();
            Container.Bind(typeof(Child)).FromComponentsInParents();

            PostInstall();

            Assert.IsEqual(_grandchild.Childs.Count, 0);
            Assert.IsEqual(_grandchild.Root, _root);
            yield break;
        }

        [UnityTest]
        public IEnumerator TestOptionalNonGeneric()
        {
            new GameObject();
            ChildWithOptional child = new GameObject().AddComponent<ChildWithOptional>();

            PreInstall();

            Container.Bind(typeof(Root)).FromComponentInParents();

            PostInstall();

            Assert.IsNull(child.Root);
            yield break;
        }

        public class Root : MonoBehaviour
        {
        }

        public class Child : MonoBehaviour
        {
        }

        public class Grandchild : MonoBehaviour
        {
            [Inject]
            public Root Root;

            [Inject]
            public List<Child> Childs;
        }

        public class Grandchild2 : MonoBehaviour
        {
            [Inject]
            public Root Root;

            [Inject]
            public Child Child;
        }

        public class ChildWithOptional : MonoBehaviour
        {
            [InjectOptional]
            public Root Root;
        }
    }
}

