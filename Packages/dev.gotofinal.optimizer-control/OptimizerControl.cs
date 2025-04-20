using System.Collections.Generic;
using UnityEngine;

namespace GotoFinal.OptimizerControl
{
    [ExecuteInEditMode]
    [AddComponentMenu("GotTools/Optimizer Control")]
    public class OptimizerControl : MonoBehaviour
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