using System.Collections.Generic;
using UnityEngine;
using VRC.SDKBase;

namespace GotoFinal.OptimizerControl
{
    [ExecuteInEditMode]
    [AddComponentMenu("GotTools/Optimizer Control")]
    public class OptimizerControlComponent : MonoBehaviour, IEditorOnly
    {
        [System.Serializable]
        public class ComponentEntry
        {
            public Component component;
            public bool removeOnFastBuild = true;
        }

        [System.Serializable]
        public class GameObjectEntry
        {
            public GameObject gameObject;
            public bool filterEnabled = false;
            public List<ComponentEntry> components = new List<ComponentEntry>();
        }

        public List<GameObjectEntry> entries = new List<GameObjectEntry>();
    }
}