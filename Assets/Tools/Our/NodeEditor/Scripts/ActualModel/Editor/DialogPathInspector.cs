
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogPath))]
public class DialogPathInspector : Editor
{
    private DialogPath state;

    void Awake()
    {
        state = (DialogPath)target;
    }

    public override void OnInspectorGUI()
    {
        string newName = EditorGUILayout.TextField("Name: ", state.name);
        if (newName != state.name)
        {
            state.name = newName;
            AssetDatabase.SaveAssets();
        }  
    }
}