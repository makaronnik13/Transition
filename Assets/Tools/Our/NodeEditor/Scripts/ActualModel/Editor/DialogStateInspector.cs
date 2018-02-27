
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogNode))]
public class DialogNodeInspector : Editor
{
    private DialogNode state;

    void Awake()
    {
        state = (DialogNode)target;
    }

    public override void OnInspectorGUI()
    {
        string newName = EditorGUILayout.TextField("Name: ",state.name);
        if (newName!=state.name)
        {
            state.name = newName;
            AssetDatabase.SaveAssets();
        }
        state.dialogNodeDescription = EditorGUILayout.TextField("Description: ", state.dialogNodeDescription, GUILayout.Height(4*EditorGUIUtility.singleLineHeight));
    }
}