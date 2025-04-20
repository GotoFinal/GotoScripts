using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GotoFinal.OptimizerControl.Editor
{
    [CustomEditor(typeof(OptimizerControl))]
    public class OptimizerControlEditor : UnityEditor.Editor
    {
        SerializedProperty entriesProp;

        void OnEnable()
        {
            entriesProp = serializedObject.FindProperty("entries");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var control = (OptimizerControl)target;

            EditorGUILayout.LabelField("List of objects/components to remove on fast build.", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("This should be a list of optimizer objects.", EditorStyles.boldLabel);

            // Render unified list
            for (int i = 0; i < entriesProp.arraySize; i++)
            {
                SerializedProperty entryProp = entriesProp.GetArrayElementAtIndex(i);
                SerializedProperty goProp = entryProp.FindPropertyRelative("gameObject");
                SerializedProperty filtProp = entryProp.FindPropertyRelative("filterEnabled");
                SerializedProperty compsProp = entryProp.FindPropertyRelative("components");

                EditorGUILayout.BeginHorizontal();
                // GameObject field
                GameObject go = (GameObject)goProp.objectReferenceValue;
                goProp.objectReferenceValue = (GameObject)EditorGUILayout.ObjectField(go, typeof(GameObject), true);

                // Filter toggle
                filtProp.boolValue = GUILayout.Toggle(filtProp.boolValue, "Component view", GUILayout.Width(120));

                // Remove button
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    entriesProp.DeleteArrayElementAtIndex(i);
                    continue;
                }

                EditorGUILayout.EndHorizontal();

                // If filtering enabled and valid GameObject, sync and show components
                if (filtProp.boolValue && go != null)
                {
                    SyncComponents(control.entries[i]);
                    EditorGUI.indentLevel++;
                    for (int j = 0; j < compsProp.arraySize; j++)
                    {
                        SerializedProperty compEntryProp = compsProp.GetArrayElementAtIndex(j);
                        SerializedProperty compProp = compEntryProp.FindPropertyRelative("component");
                        SerializedProperty incProp = compEntryProp.FindPropertyRelative("removeOnFastBuild");

                        Component comp = (Component)compProp.objectReferenceValue;
                        string label = comp != null ? comp.GetType().Name : "(missing)";

                        EditorGUILayout.BeginHorizontal();
                        incProp.boolValue = GUILayout.Toggle(incProp.boolValue, label);
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                }
            }

            // Add new GameObject slot
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Add Object:", GUILayout.Width(80));
            GameObject newGo = (GameObject)EditorGUILayout.ObjectField(null, typeof(GameObject), true);
            if (newGo != null)
            {
                var newEntry = new OptimizerControl.GameObjectEntry();
                newEntry.gameObject = newGo;
                newEntry.filterEnabled = false;
                control.entries.Add(newEntry);
                SyncComponents(newEntry);
            }

            EditorGUILayout.EndHorizontal();

            if (GUI.changed)
                EditorUtility.SetDirty(target);

            serializedObject.ApplyModifiedProperties();
        }

        void SyncComponents(OptimizerControl.GameObjectEntry entry)
        {
            var go = entry.gameObject;
            if (go == null) return;

            // Remove stale
            entry.components.RemoveAll(c => c.component == null || !go.GetComponents<Component>().Contains(c.component));
            // Add new
            foreach (var comp in go.GetComponents<Component>())
            {
                if (comp.GetType() == typeof(Transform)) continue;
                if (!entry.components.Any(c => c.component == comp))
                {
                    entry.components.Add(new OptimizerControl.ComponentEntry
                    {
                        component = comp,
                        removeOnFastBuild = true
                    });
                }
            }
        }
    }
}