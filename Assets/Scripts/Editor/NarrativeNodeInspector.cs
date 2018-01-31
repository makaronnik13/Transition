using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NarrativeNode))]
public class NarrativeNodeInspector : Editor
{
    private NarrativeNode node;
    private NarrativeNode Node
    {
        get
        {
            if (node==null)
            {
                node = (NarrativeNode)target;
            }
            return node;
        }
    }

    public override void OnInspectorGUI()
    {

        EditorGUILayout.BeginVertical();

        string newName = EditorGUILayout.DelayedTextField(Node.name);

        if (newName!=Node.name)
        {
            Node.name = newName;
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(Node));
        }

        Node.Description = EditorGUILayout.TextArea(Node.Description, GUILayout.Height(50));

        if (GUILayout.Button("Edit"))
        {
            NodeEditor.ShowNode((NarrativeNode)target);
        }

        EditorGUILayout.EndVertical();
    }
}