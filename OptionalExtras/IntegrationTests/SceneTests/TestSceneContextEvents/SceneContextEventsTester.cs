using ModestTree;
using UnityEngine;

namespace Zenject.Tests
{
    public class SceneContextEventsTester : MonoBehaviour
    {
        [SerializeField]
        private SceneContext _sceneContext = null;
        private bool _calledPreInstall;
        private bool _calledPostInstall;
        private bool _calledPreResolve;
        private bool _calledPostResolve;

        public void Awake()
        {
            Assert.That(!_sceneContext.HasResolved);
            Assert.That(!_sceneContext.HasInstalled);

            _sceneContext.PreInstall += OnPreInstall;
            _sceneContext.PostInstall += OnPostInstall;
            _sceneContext.PreResolve += OnPreResolve;
            _sceneContext.PostResolve += OnPostResolve;
        }

        public void Start()
        {
            Assert.That(_calledPreInstall);
            Assert.That(_calledPostInstall);
            Assert.That(_calledPreResolve);
            Assert.That(_calledPostResolve);
        }

        private void OnPreInstall()
        {
            _calledPreInstall = true;
            Assert.IsNotNull(_sceneContext.Container);
        }

        private void OnPostInstall()
        {
            _calledPostInstall = true;
        }

        private void OnPreResolve()
        {
            _calledPreResolve = true;
        }

        private void OnPostResolve()
        {
            _calledPostResolve = true;
        }
    }
}
