using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GraphEditor
{
    [CustomEditor(typeof(NodeGraph))]
    public class NodeGraphInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Edit"))
            {
                DialogEditor.Init((NodeGraph)target);
            }
        }
    }
}
