#if !NOT_UNITY3D

using System;
using UnityEngine;

namespace Zenject
{
    public abstract class ScriptableObjectInstallerBase : ScriptableObject, IInstaller
    {
        [Inject]
        private DiContainer _container = null;

        protected DiContainer Container
        {
            get { return _container; }
        }

        public abstract void InstallBindings();
    }
}

#endif

