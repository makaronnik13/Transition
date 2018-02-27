
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogStatePath))]
public class DialogStatePathInspector : Editor
{
    private DialogStatePath state;

    void Awake()
    {
        state = (DialogStatePath)target;
    }

    public override void OnInspectorGUI()
    {
        string newName = EditorGUILayout.TextField("Name: ", state.name);
        if (newName != state.name)
        {
            state.name = newName;
            AssetDatabase.SaveAssets();
        }
        state.text = EditorGUILayout.TextField("Text: ", state.text, GUILayout.Height(4 * EditorGUIUtility.singleLineHeight));
    }
}
