using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Zenject.ReflectionBaking
{
    [CustomEditor(typeof(ZenjectReflectionBakingSettings))]
    public class ZenjectReflectionBakingSettingsEditor : Editor
    {
        private SerializedProperty _includeAssemblies;
        private SerializedProperty _excludeAssemblies;
        private SerializedProperty _namespacePatterns;
        private SerializedProperty _isEnabledInBuilds;
        private SerializedProperty _isEnabledInEditor;
        private SerializedProperty _allGeneratedAssemblies;

        // Lists
        private ReorderableList _includeAssembliesList;
        private ReorderableList _excludeAssembliesList;
        private ReorderableList _namespacePatternsList;

        // Layouts
        private Vector2 _logScrollPosition;
        private int _selectedLogIndex;
        private bool _hasModifiedProperties;
        private static GUIContent _includeAssembliesListHeaderContent = new GUIContent
        {
            text = "Include Assemblies",
            tooltip = "The list of all the assemblies that will be editted to have reflection information directly embedded"
        };
        private static GUIContent _excludeAssembliesListHeaderContent = new GUIContent
        {
            text = "Exclude Assemblies",
            tooltip = "The list of all the assemblies that will not be editted"
        };
        private static GUIContent _namespacePatternListHeaderContent = new GUIContent
        {
            text = "Namespace Patterns",
            tooltip = "This list of Regex patterns will be compared to the name of each type in the given assemblies, and when a match is found that type will be editting to directly contain reflection information"
        };

        private void OnEnable()
        {
            _includeAssemblies = serializedObject.FindProperty("_includeAssemblies");
            _excludeAssemblies = serializedObject.FindProperty("_excludeAssemblies");
            _namespacePatterns = serializedObject.FindProperty("_namespacePatterns");
            _isEnabledInEditor = serializedObject.FindProperty("_isEnabledInEditor");
            _isEnabledInBuilds = serializedObject.FindProperty("_isEnabledInBuilds");
            _allGeneratedAssemblies = serializedObject.FindProperty("_allGeneratedAssemblies");

            _namespacePatternsList = new ReorderableList(serializedObject, _namespacePatterns);
            _namespacePatternsList.drawHeaderCallback += OnNamespacePatternsDrawHeader;
            _namespacePatternsList.drawElementCallback += OnNamespacePatternsDrawElement;

            _includeAssembliesList = new ReorderableList(serializedObject, _includeAssemblies);
            _includeAssembliesList.drawHeaderCallback += OnIncludeWeavedAssemblyDrawHeader;
            _includeAssembliesList.onAddCallback += OnIncludeWeavedAssemblyElementAdded;
            _includeAssembliesList.drawElementCallback += OnIncludeAssemblyListDrawElement;

            _excludeAssembliesList = new ReorderableList(serializedObject, _excludeAssemblies);
            _excludeAssembliesList.drawHeaderCallback += OnExcludeWeavedAssemblyDrawHeader;
            _excludeAssembliesList.onAddCallback += OnExcludeWeavedAssemblyElementAdded;
            _excludeAssembliesList.drawElementCallback += OnExcludeAssemblyListDrawElement;
        }

        private void OnNamespacePatternsDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty indexProperty = _namespacePatterns.GetArrayElementAtIndex(index);
            indexProperty.stringValue = EditorGUI.TextField(rect, indexProperty.stringValue);
        }

        private void OnExcludeAssemblyListDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty indexProperty = _excludeAssemblies.GetArrayElementAtIndex(index);
            EditorGUI.LabelField(rect, indexProperty.stringValue, EditorStyles.textArea);
        }

        private void OnIncludeAssemblyListDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty indexProperty = _includeAssemblies.GetArrayElementAtIndex(index);
            EditorGUI.LabelField(rect, indexProperty.stringValue, EditorStyles.textArea);
        }

        private void OnNamespacePatternsDrawHeader(Rect rect)
        {
            GUI.Label(rect, _namespacePatternListHeaderContent);
        }

        private void OnExcludeWeavedAssemblyDrawHeader(Rect rect)
        {
            GUI.Label(rect, _excludeAssembliesListHeaderContent);
        }

        private void OnIncludeWeavedAssemblyDrawHeader(Rect rect)
        {
            GUI.Label(rect, _includeAssembliesListHeaderContent);
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            {
                GUILayout.Label("Settings", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(_isEnabledInBuilds, true);

                bool oldIsEnabledInEditorValue = _isEnabledInEditor.boolValue;
                EditorGUILayout.PropertyField(_isEnabledInEditor, true);

                if (oldIsEnabledInEditorValue != _isEnabledInEditor.boolValue)
                {
                    ReflectionBakingInternalUtil.TryForceUnityFullCompile();
                }

#if !UNITY_2018_1_OR_NEWER
                if (_isEnabledInEditor.boolValue)
                {
                    EditorGUILayout.HelpBox(
                        "Reflection baking inside unity editor requires Unity 2018+!  It is however supported for builds", MessageType.Error);
                }
#endif
                EditorGUILayout.PropertyField(_allGeneratedAssemblies, true);

                if (_allGeneratedAssemblies.boolValue)
                {
                    _excludeAssembliesList.DoLayoutList();

                    GUI.enabled = false;

                    try
                    {
                        _includeAssembliesList.DoLayoutList();
                    }
                    finally
                    {
                        GUI.enabled = true;
                    }
                }
                else
                {
                    GUI.enabled = false;

                    try
                    {
                        _excludeAssembliesList.DoLayoutList();
                    }
                    finally
                    {
                        GUI.enabled = true;
                    }

                    _includeAssembliesList.DoLayoutList();
                }

                _namespacePatternsList.DoLayoutList();
            }

            if (EditorGUI.EndChangeCheck())
            {
                _hasModifiedProperties = true;
            }

            if (_hasModifiedProperties)
            {
                _hasModifiedProperties = false;
                ApplyModifiedProperties();
            }
        }

        private void ApplyModifiedProperties()
        {
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private void OnExcludeWeavedAssemblyElementAdded(ReorderableList list)
        {
            OnAssemblyElementAdded(_excludeAssemblies, list);
        }

        private void OnIncludeWeavedAssemblyElementAdded(ReorderableList list)
        {
            OnAssemblyElementAdded(_includeAssemblies, list);
        }

        private void OnAssemblyElementAdded(SerializedProperty listProperty, ReorderableList list)
        {
            var menu = new GenericMenu();

            System.Collections.Generic.List<string> paths = AssemblyPathRegistry.GetAllGeneratedAssemblyRelativePaths();

            for (int i = 0; i < paths.Count; i++)
            {
                string path = paths[i];

                bool foundMatch = false;

                for (int k = 0; k < listProperty.arraySize; k++)
                {
                    SerializedProperty current = listProperty.GetArrayElementAtIndex(k);

                    if (path == current.stringValue.Replace("/", "\\"))
                    {
                        foundMatch = true;
                        break;
                    }
                }

                if (!foundMatch)
                {
                    var content = new GUIContent(path);
                    menu.AddItem(content, false, p => OnWeavedAssemblyAdded(listProperty, p), path);
                }
            }

            if (menu.GetItemCount() == 0)
            {
                menu.AddDisabledItem(new GUIContent("[All Assemblies Added]"));
            }

            menu.ShowAsContext();
        }

        private void OnWeavedAssemblyAdded(SerializedProperty listProperty, object path)
        {
            listProperty.arraySize++;
            SerializedProperty weaved = listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
            weaved.stringValue = ((string)path).Replace("\\", "/");
            ApplyModifiedProperties();
        }
    }
}
