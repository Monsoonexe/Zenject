#if !NOT_UNITY3D

using ModestTree;
using UnityEngine;

namespace Zenject
{
    public abstract class ScriptableObjectInstaller : ScriptableObjectInstallerBase
    {
    }

    //
    // Derive from this class instead to install like this:
    //     FooInstaller.InstallFromResource(Container);
    // Or
    //     FooInstaller.InstallFromResource("My/Path/ToScriptableObjectInstance", Container);
    //
    // (Instead of needing to add the ScriptableObjectInstaller directly via inspector)
    //
    // This approach is needed if you want to pass in strongly typed runtime parameters too it
    //
    public abstract class ScriptableObjectInstaller<TDerived> : ScriptableObjectInstaller
        where TDerived : ScriptableObjectInstaller<TDerived>
    {
        public static TDerived InstallFromResource(DiContainer container)
        {
            return InstallFromResource(
                ScriptableObjectInstallerUtil.GetDefaultResourcePath<TDerived>(), container);
        }

        public static TDerived InstallFromResource(string resourcePath, DiContainer container)
        {
            TDerived installer = ScriptableObjectInstallerUtil.CreateInstaller<TDerived>(resourcePath);
            container.Inject(installer);
            installer.InstallBindings();
            return installer;
        }
    }

    public abstract class ScriptableObjectInstaller<TParam1, TDerived> : ScriptableObjectInstallerBase
        where TDerived : ScriptableObjectInstaller<TParam1, TDerived>
    {
        public static TDerived InstallFromResource(DiContainer container, TParam1 p1)
        {
            return InstallFromResource(
                ScriptableObjectInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1);
        }

        public static TDerived InstallFromResource(string resourcePath, DiContainer container, TParam1 p1)
        {
            TDerived installer = ScriptableObjectInstallerUtil.CreateInstaller<TDerived>(resourcePath);
            container.InjectExplicit(installer, InjectUtil.CreateArgListExplicit(p1));
            installer.InstallBindings();
            return installer;
        }
    }

    public abstract class ScriptableObjectInstaller<TParam1, TParam2, TDerived> : ScriptableObjectInstallerBase
        where TDerived : ScriptableObjectInstaller<TParam1, TParam2, TDerived>
    {
        public static TDerived InstallFromResource(DiContainer container, TParam1 p1, TParam2 p2)
        {
            return InstallFromResource(
                ScriptableObjectInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1, p2);
        }

        public static TDerived InstallFromResource(string resourcePath, DiContainer container, TParam1 p1, TParam2 p2)
        {
            TDerived installer = ScriptableObjectInstallerUtil.CreateInstaller<TDerived>(resourcePath);
            container.InjectExplicit(installer, InjectUtil.CreateArgListExplicit(p1, p2));
            installer.InstallBindings();
            return installer;
        }
    }

    public abstract class ScriptableObjectInstaller<TParam1, TParam2, TParam3, TDerived> : ScriptableObjectInstallerBase
        where TDerived : ScriptableObjectInstaller<TParam1, TParam2, TParam3, TDerived>
    {
        public static TDerived InstallFromResource(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3)
        {
            return InstallFromResource(
                ScriptableObjectInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1, p2, p3);
        }

        public static TDerived InstallFromResource(string resourcePath, DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3)
        {
            TDerived installer = ScriptableObjectInstallerUtil.CreateInstaller<TDerived>(resourcePath);
            container.InjectExplicit(installer, InjectUtil.CreateArgListExplicit(p1, p2, p3));
            installer.InstallBindings();
            return installer;
        }
    }

    public abstract class ScriptableObjectInstaller<TParam1, TParam2, TParam3, TParam4, TDerived> : ScriptableObjectInstallerBase
        where TDerived : ScriptableObjectInstaller<TParam1, TParam2, TParam3, TParam4, TDerived>
    {
        public static TDerived InstallFromResource(DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            return InstallFromResource(
                ScriptableObjectInstallerUtil.GetDefaultResourcePath<TDerived>(), container, p1, p2, p3, p4);
        }

        public static TDerived InstallFromResource(string resourcePath, DiContainer container, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            TDerived installer = ScriptableObjectInstallerUtil.CreateInstaller<TDerived>(resourcePath);
            container.InjectExplicit(installer, InjectUtil.CreateArgListExplicit(p1, p2, p3, p4));
            installer.InstallBindings();
            return installer;
        }
    }

    public static class ScriptableObjectInstallerUtil
    {
        public static string GetDefaultResourcePath<TInstaller>()
            where TInstaller : ScriptableObjectInstallerBase
        {
            return "Installers/" + typeof(TInstaller).PrettyName();
        }

        public static TInstaller CreateInstaller<TInstaller>(string resourcePath)
            where TInstaller : ScriptableObjectInstallerBase
        {
            Object[] installers = Resources.LoadAll(resourcePath);

            Assert.That(installers.Length == 1,
                "Could not find unique ScriptableObjectInstaller with type '{0}' at resource path '{1}'", typeof(TInstaller), resourcePath);

            Object installer = installers[0];

            Assert.That(installer is TInstaller,
                "Expected to find installer with type '{0}' at resource path '{1}'", typeof(TInstaller), resourcePath);

            return (TInstaller)installer;
        }
    }
}

#endif
