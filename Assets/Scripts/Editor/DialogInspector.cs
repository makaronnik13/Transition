using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PersonDialogs))]
public class DialogInspector : Editor
{

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Edit"))
        {
            NodeEditor.Init((PersonDialogs)target);
        }

        
    }
}
