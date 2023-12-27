using System.Collections.Generic;
using System.Linq;

namespace Zenject
{
    public static class CompositeInstallerExtensions
    {
        public static bool ValidateLeafInstallers<T>(this ICompositeInstaller<T> compositeInstaller)
            where T : IInstaller
        {
            IReadOnlyList<T> leafInstallers = compositeInstaller.LeafInstallers;
            for (int i = 0; i < leafInstallers.Count; ++i)
            {
                T leafInstaller = leafInstallers[i];
                bool leafResult = leafInstaller.ValidateAsComposite(compositeInstaller);
                if (!leafResult)
                {
                    return false;
                }
            }

            return true;
        }

        // Specify T if the installer is concrete composite installer so that T will not be infered as ICompositeInstaller.
        public static bool ValidateAsComposite<T>(this T installer)
            where T : IInstaller
        {
            if (!(installer is ICompositeInstaller<T> compositeInstaller))
            {
                return true;
            }

            IReadOnlyList<T> leafInstallers = compositeInstaller.LeafInstallers;
            for (int i = 0; i < leafInstallers.Count; ++i)
            {
                T leafInstaller = leafInstallers[i];
                bool leafResult = leafInstaller.ValidateAsComposite(compositeInstaller);
                if (!leafResult)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ValidateAsComposite<T>(this T installer, ICompositeInstaller<T> parent1)
            where T : IInstaller
        {
            if (!(installer is ICompositeInstaller<T> compositeInstaller))
            {
                return true;
            }

            if (compositeInstaller == parent1)
            {
                return false;
            }

            IReadOnlyList<T> leafInstallers = compositeInstaller.LeafInstallers;
            for (int i = 0; i < leafInstallers.Count; ++i)
            {
                T leafInstaller = leafInstallers[i];
                bool leafResult = leafInstaller.ValidateAsComposite(parent1, compositeInstaller);
                if (!leafResult)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ValidateAsComposite<T>(this T installer, ICompositeInstaller<T> parent1, ICompositeInstaller<T> parent2)
            where T : IInstaller
        {
            if (!(installer is ICompositeInstaller<T> compositeInstaller))
            {
                return true;
            }

            if (compositeInstaller == parent1 ||
                compositeInstaller == parent2)
            {
                return false;
            }

            IReadOnlyList<T> leafInstallers = compositeInstaller.LeafInstallers;
            for (int i = 0; i < leafInstallers.Count; ++i)
            {
                T leafInstaller = leafInstallers[i];
                bool leafResult = leafInstaller.ValidateAsComposite(parent1, parent2, compositeInstaller);
                if (!leafResult)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ValidateAsComposite<T>(this T installer, ICompositeInstaller<T> parent1, ICompositeInstaller<T> parent2, ICompositeInstaller<T> parent3)
            where T : IInstaller
        {
            if (!(installer is ICompositeInstaller<T> compositeInstaller))
            {
                return true;
            }

            if (compositeInstaller == parent1 ||
                compositeInstaller == parent2 ||
                compositeInstaller == parent3)
            {
                return false;
            }

            IReadOnlyList<T> leafInstallers = compositeInstaller.LeafInstallers;
            for (int i = 0; i < leafInstallers.Count; ++i)
            {
                T leafInstaller = leafInstallers[i];
                bool leafResult = leafInstaller.ValidateAsComposite(parent1, parent2, parent3, compositeInstaller);
                if (!leafResult)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ValidateAsComposite<T>(this T installer, ICompositeInstaller<T> parent1, ICompositeInstaller<T> parent2, ICompositeInstaller<T> parent3, ICompositeInstaller<T> parent4)
            where T : IInstaller
        {
            if (!(installer is ICompositeInstaller<T> compositeInstaller))
            {
                return true;
            }

            if (compositeInstaller == parent1 ||
                compositeInstaller == parent2 ||
                compositeInstaller == parent3 ||
                compositeInstaller == parent4)
            {
                return false;
            }

            var childParentInstallers = new List<ICompositeInstaller<T>>(8)
            {
                parent1,
                parent2,
                parent3,
                parent4,
                compositeInstaller,
            };

            IReadOnlyList<T> leafInstallers = compositeInstaller.LeafInstallers;
            for (int i = 0; i < leafInstallers.Count; ++i)
            {
                T leafInstaller = leafInstallers[i];
                bool leafResult = leafInstaller.ValidateAsCompositeSavedAlloc(childParentInstallers);
                if (!leafResult)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ValidateAsComposite<T>(
            this T leafInstaller,
            IReadOnlyList<ICompositeInstaller<T>> parentInstallers)
            where T : IInstaller
        {
            if (!(leafInstaller is ICompositeInstaller<T> compositeInstaller))
            {
                return true;
            }

            if (parentInstallers.Contains(compositeInstaller))
            {
                // Found a circular reference
                return false;
            }

            var childParentInstallers = new List<ICompositeInstaller<T>>(parentInstallers)
            {
                compositeInstaller
            };

            bool result = compositeInstaller
                .LeafInstallers
                .All(installer => installer.ValidateAsCompositeSavedAlloc(childParentInstallers));
            return result;
        }

        public static bool ValidateAsCompositeSavedAlloc<T>(
            this T leafInstaller,
            List<ICompositeInstaller<T>> reusableParentInstallers)
            where T : IInstaller
        {
            if (!(leafInstaller is ICompositeInstaller<T> compositeInstaller))
            {
                return true;
            }

            if (reusableParentInstallers.Contains(compositeInstaller))
            {
                // Found a circular reference
                return false;
            }

            bool result = true;

            int compositeInstallerIndex = reusableParentInstallers.Count;
            reusableParentInstallers.Add(compositeInstaller);

            IReadOnlyList<T> leafInstallers = compositeInstaller.LeafInstallers;
            for (int i = 0; i < leafInstallers.Count; ++i)
            {
                T installer = leafInstallers[i];
                result &= installer.ValidateAsCompositeSavedAlloc(reusableParentInstallers);

                if (!result)
                {
                    break;
                }
            }

            reusableParentInstallers.RemoveAt(compositeInstallerIndex);

            return result;
        }
    }
}