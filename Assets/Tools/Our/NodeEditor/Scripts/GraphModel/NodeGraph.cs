using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphEditor
{
    [System.Serializable]
    public class NodeGraph : ScriptableObject
    {
        [SerializeField]
        public List<Node> nodes = new List<Node>();
    }
}
