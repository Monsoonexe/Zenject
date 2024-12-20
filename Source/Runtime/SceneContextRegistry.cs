using ModestTree;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Zenject
{
    public class SceneContextRegistry
    {
        private readonly Dictionary<Scene, SceneContext> _map = new Dictionary<Scene, SceneContext>();

        [Inject]
        public SceneContextRegistry()
        {
        }

        public IEnumerable<SceneContext> SceneContexts
        {
            get { return _map.Values; }
        }

        public void Add(SceneContext context)
        {
            Assert.That(!_map.ContainsKey(context.gameObject.scene));
            _map.Add(context.gameObject.scene, context);
        }

        public SceneContext GetSceneContextForScene(string name)
        {
            Scene scene = SceneManager.GetSceneByName(name);
            Assert.That(scene.IsValid(), "Could not find scene with name '{0}'", name);
            return GetSceneContextForScene(scene);
        }

        public SceneContext GetSceneContextForScene(Scene scene)
        {
            return _map[scene];
        }

        public SceneContext TryGetSceneContextForScene(string name)
        {
            Scene scene = SceneManager.GetSceneByName(name);
            Assert.That(scene.IsValid(), "Could not find scene with name '{0}'", name);
            return TryGetSceneContextForScene(scene);
        }

        public SceneContext TryGetSceneContextForScene(Scene scene)
        {
            SceneContext context;

            return _map.TryGetValue(scene, out context) ? context : null;
        }

        public DiContainer GetContainerForScene(Scene scene)
        {
            DiContainer container = TryGetContainerForScene(scene);

            return container != null
                ? container
                : throw Assert.CreateException(
                "Unable to find DiContainer for scene '{0}'", scene.name);
        }

        public DiContainer TryGetContainerForScene(Scene scene)
        {
            if (scene == ProjectContext.Instance.gameObject.scene)
            {
                return ProjectContext.Instance.Container;
            }

            SceneContext sceneContext = TryGetSceneContextForScene(scene);

            return sceneContext != null ? sceneContext.Container : null;
        }

        public void Remove(SceneContext context)
        {
            bool removed = _map.Remove(context.gameObject.scene);

            if (!removed)
            {
                Log.Warn("Failed to remove SceneContext from SceneContextRegistry");
            }
        }
    }

}
