using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace GotoFinal.ObjectRemover
{
    [ExecuteInEditMode]
    [AddComponentMenu("GotTools/Object Remover")]
    public class ObjectRemoverComponent : MonoBehaviour
    {
        public bool hideInHierarchy = true;
        public bool keepDisabled = true;
        public bool markAsEditorOnly = true;
        public List<GameObject> objectsToRemove = new List<GameObject>();
        private List<GameObject> _lastObjectsToRemove;

#if UNITY_EDITOR
        private void OnEnable()
        {
            OnListChange();
        }

        private new void OnDestroy()
        {
            if (Application.isPlaying) return;
            hideInHierarchy = false;
            markAsEditorOnly = false;
            Unhide();
            UnmarkAsEditorOnly();
            objectsToRemove.Clear();
            _lastObjectsToRemove = null;
        }

        public void PreListChange()
        {
            // uh, i wonder if there is better way that actually works
            if (!DidListChange()) return;
            _lastObjectsToRemove = new List<GameObject>(objectsToRemove);
        }

        public void PostListChange()
        {
            // i hate unity so much
            if (!DidListChange()) return;
            OnListChange();
        }

        private bool DidListChange()
        {
            if (_lastObjectsToRemove == null) return false;
            if (_lastObjectsToRemove.Count != objectsToRemove.Count) return true;
            for (var i = _lastObjectsToRemove.Count - 1; i >= 0; i--)
            {
                if (_lastObjectsToRemove[i] != objectsToRemove[i]) return true;
            }

            return false;
        }

        public void OnListChange()
        {
            objectsToRemove.RemoveAll(x => !x);
            if (Application.isPlaying) return;
            if (_lastObjectsToRemove != null)
            {
                foreach (var newValue in objectsToRemove) _lastObjectsToRemove.Remove(newValue);
                foreach (var removedFromList in _lastObjectsToRemove)
                {
                    if (!removedFromList) continue;
                    removedFromList.hideFlags &= ~HideFlags.HideInHierarchy;
                    UnmarkAsEditorOnly(removedFromList);
                }
            }

            _lastObjectsToRemove = new List<GameObject>(objectsToRemove);
            if (hideInHierarchy) Hide();
            if (markAsEditorOnly) MarkAsEditorOnly();
            if (keepDisabled)
            {
                foreach (var obj in objectsToRemove) obj.SetActive(false);
            }
        }

        public void MarkAsEditorOnly()
        {
            foreach (var obj in objectsToRemove)
            {
                obj.tag = "EditorOnly";
            }
        }

        public void UnmarkAsEditorOnly()
        {
            foreach (var obj in objectsToRemove)
            {
                UnmarkAsEditorOnly(obj);
            }
        }

        public void UnmarkAsEditorOnly(GameObject obj)
        {
            if (PrefabUtility.IsPartOfPrefabInstance(obj))
            {
                SerializedObject so = new SerializedObject(obj);
                SerializedProperty tagProp = so.FindProperty("m_TagString");
                if (tagProp != null && PrefabUtility.HasPrefabInstanceAnyOverrides(obj, false))
                {
                    PrefabUtility.RevertPropertyOverride(tagProp, InteractionMode.AutomatedAction);
                }
            }
            else
            {
                obj.tag = "Untagged";
            }
        }

        public void Hide()
        {
            foreach (var obj in objectsToRemove)
            {
                if (IsSafeToBeHidden(obj)) obj.hideFlags |= HideFlags.HideInHierarchy;
            }
        }

        public void Unhide()
        {
            foreach (var obj in objectsToRemove)
            {
                obj.hideFlags &= ~HideFlags.HideInHierarchy;
            }
        }

        private bool IsSafeToBeHidden(GameObject obj)
        {
            return obj.GetComponentInChildren<ObjectRemoverComponent>(true) == null;
        }
#endif
    }
}