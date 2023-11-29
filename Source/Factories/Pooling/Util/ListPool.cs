using System.Collections.Generic;

namespace Zenject
{
    public class ListPool<T> : StaticMemoryPool<List<T>>
    {
        private static ListPool<T> _instance = new ListPool<T>();

        public ListPool()
        {
            OnDespawnedMethod = OnDespawned;
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

        public static ListPool<T> Instance
        {
            get { return _instance; }
        }

        private void OnDespawned(List<T> list)
        {
            list.Clear();
        }
    }
}
