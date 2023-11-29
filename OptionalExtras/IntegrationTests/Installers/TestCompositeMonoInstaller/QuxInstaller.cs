using UnityEngine;

namespace Zenject.Tests.Installers.CompositeMonoInstallers
{
    public class QuxInstaller : MonoInstaller<QuxInstaller>
    {
        [SerializeField] private string _p1;
        [SerializeField] private float _p2;
        [SerializeField] private int _p3;

        public override void InstallBindings()
        {
            Container.BindInstance(_p1);
            Container.BindInstance(_p2);
            Container.BindInstance(_p3);
        }
    }
}
