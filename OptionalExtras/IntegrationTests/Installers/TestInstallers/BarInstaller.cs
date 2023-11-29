namespace Zenject.Tests.Installers.Installers
{
    public class BarInstaller : Installer<string, BarInstaller>
    {
        private string _value;

        [Inject]
        public void Construct(string value)
        {
            _value = value;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(_value);
        }
    }
}
