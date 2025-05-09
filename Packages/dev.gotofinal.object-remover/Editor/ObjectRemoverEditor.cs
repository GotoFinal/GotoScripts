﻿using UnityEditor;
using UnityEngine;

namespace GotoFinal.ObjectRemover
{
    [CustomEditor(typeof(ObjectRemoverComponent))]
    internal class ObjectRemoverEditor : UnityEditor.Editor
    {
        private SerializedProperty prop_hideInHierarchy;
        private SerializedProperty prop_keepDisabled;
        private SerializedProperty prop_markAsEditorOnly;
        private SerializedProperty prop_objectsToRemove;

        private void OnEnable()
        {
            prop_hideInHierarchy = serializedObject.FindProperty(nameof(ObjectRemoverComponent.hideInHierarchy));
            prop_keepDisabled = serializedObject.FindProperty(nameof(ObjectRemoverComponent.keepDisabled));
            prop_markAsEditorOnly = serializedObject.FindProperty(nameof(ObjectRemoverComponent.markAsEditorOnly));
            prop_objectsToRemove = serializedObject.FindProperty(nameof(ObjectRemoverComponent.objectsToRemove));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var remover = (ObjectRemoverComponent)target;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(prop_hideInHierarchy, new GUIContent("Hide In Hierarchy"));
            bool hideChanged = EditorGUI.EndChangeCheck();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(prop_markAsEditorOnly, new GUIContent("Mark as Editor Only"));
            bool removeChanged = EditorGUI.EndChangeCheck();

            EditorGUILayout.PropertyField(prop_keepDisabled, new GUIContent("Keep Disabled"));

            remover.PreListChange();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(prop_objectsToRemove, new GUIContent("Objects To Remove"), true);
            bool listChanged = EditorGUI.EndChangeCheck();
            
            serializedObject.ApplyModifiedProperties();
            remover.PostListChange(); // unity is bad.

            if (hideChanged || removeChanged || listChanged) remover.OnListChange();
            if (hideChanged && !prop_hideInHierarchy.boolValue) remover.Unhide();
            if (removeChanged && !prop_markAsEditorOnly.boolValue) remover.UnmarkAsEditorOnly();
        }
    }
}