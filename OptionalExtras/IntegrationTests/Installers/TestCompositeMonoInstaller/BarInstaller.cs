using UnityEngine;

namespace Zenject.Tests.Installers.CompositeMonoInstallers
{
    public class BarInstaller : MonoInstaller<BarInstaller>
    {
        [SerializeField] private string _value;

        public override void InstallBindings()
        {
            Container.BindInstance(_value);
        }
    }
}
