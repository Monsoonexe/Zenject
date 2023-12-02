using ModestTree;
using System.Collections.Generic;

namespace Zenject
{
    public class HashSetPool<T> : StaticMemoryPool<HashSet<T>>
    {
        private static readonly HashSetPool<T> _instance = new HashSetPool<T>();

        public HashSetPool()
        {
#if !ZEN_STRIP_ASSERTS_IN_BUILDS
            OnSpawnMethod = OnSpawned;
#endif
            OnDespawnedMethod = OnDespawned;
        }

        public static HashSetPool<T> Instance
        {
            get { return _instance; }
        }

#if UNITY_EDITOR
        // Required for disabling domain reload in enter the play mode feature. See: https://docs.unity3d.com/Manual/DomainReloading.html
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticValues()
        {
            if (!UnityEditor.EditorSettings.enterPlayModeOptionsEnabled)
            {
                return;
            }

            _instance.Clear();
        }
#endif

        private static void OnSpawned(HashSet<T> items)
        {
            Assert.That(items.IsEmpty());
        }

        private static void OnDespawned(HashSet<T> items)
        {
            items.Clear();
        }
    }
}
