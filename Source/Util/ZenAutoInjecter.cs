using ModestTree;
using UnityEngine;

namespace Zenject
{
    public class ZenAutoInjecter : MonoBehaviour
    {
        [SerializeField]
        private ContainerSources _containerSource = ContainerSources.SearchHierarchy;
        private bool _hasInjected;

        public ContainerSources ContainerSource
        {
            get { return _containerSource; }
            set { _containerSource = value; }
        }

        // Make sure they don't cause injection to happen twice
        [Inject]
        public void Construct()
        {
            if (!_hasInjected)
            {
                throw Assert.CreateException(
                    "ZenAutoInjecter was injected!  Do not use ZenAutoInjecter for objects that are instantiated through zenject or which exist in the initial scene hierarchy");
            }
        }

        public void Awake()
        {
            _hasInjected = true;
            LookupContainer().InjectGameObject(gameObject);
        }

        private DiContainer LookupContainer()
        {
            if (_containerSource == ContainerSources.ProjectContext)
            {
                return ProjectContext.Instance.Container;
            }

            if (_containerSource == ContainerSources.SceneContext)
            {
                return GetContainerForCurrentScene();
            }

            Assert.IsEqual(_containerSource, ContainerSources.SearchHierarchy);

            Context parentContext = transform.GetComponentInParent<Context>();

            return parentContext != null ? parentContext.Container : GetContainerForCurrentScene();
        }

        private DiContainer GetContainerForCurrentScene()
        {
            return ProjectContext.Instance.Container.Resolve<SceneContextRegistry>()
                .GetContainerForScene(gameObject.scene);
        }

        public enum ContainerSources
        {
            SceneContext,
            ProjectContext,
            SearchHierarchy
        }
    }
}
