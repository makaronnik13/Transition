
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Linq;

[CustomEditor(typeof(DialogStateNode))]
public class DialogStateNodeInspector : Editor
{
    private DialogStateNode state;
	private ReorderableList changersList;

    void Awake()
    {
        state = (DialogStateNode)target;
		CreateChangersList ();
    }

    public override void OnInspectorGUI()
    {
		EditorGUI.BeginDisabledGroup (state.nodeType != DialogStateNode.StateNodeType.simple);

        string newName = EditorGUILayout.TextField("Name: ", state.name);
        if (newName != state.name)
        {
            state.name = newName;
            AssetDatabase.SaveAssets();
        }

		state.withEvent = EditorGUILayout.Toggle ("With event:", state.withEvent);
		EditorGUI.EndDisabledGroup ();

		if(state.nodeType == DialogStateNode.StateNodeType.simple)
		{
        	state.text = EditorGUILayout.TextField("Text: ", state.text, GUILayout.Height(4 * EditorGUIUtility.singleLineHeight));
		}

		state.comentary = EditorGUILayout.TextField ("Comentary: ", state.comentary, GUILayout.Height (2 * EditorGUIUtility.singleLineHeight));


		serializedObject.Update();
		changersList.DoLayoutList ();
		serializedObject.ApplyModifiedProperties();
	}

	private void CreateChangersList()
	{
		changersList = new ReorderableList(serializedObject, 
			serializedObject.FindProperty("effects"), 
			true, true, true, true);
		changersList.drawElementCallback =  
			(Rect rect, int index, bool isActive, bool isFocused) => {
			var element =  changersList.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += 2;

			Dialog dialog = AssetDatabase.LoadAssetAtPath<Dialog>(AssetDatabase.GetAssetPath(state)); 
			if(dialog.paramCollection.Parameters.Count>0)
			{
				int paramIndex = 0;

				if(!dialog.paramCollection.Parameters.Contains(state.effects[index].parameter))
				{
					state.effects[index].parameter = dialog.paramCollection.Parameters[0];
				}


				paramIndex = EditorGUI.Popup(
					new Rect(rect.x, rect.y, 160, EditorGUIUtility.singleLineHeight),
					dialog.paramCollection.Parameters.IndexOf(state.effects[index].parameter), dialog.paramCollection.Parameters.Select(p=>p.name).ToArray());


				state.effects[index].parameter = dialog.paramCollection.Parameters[paramIndex];

			}
			else
			{
				state.effects[index].parameter = null;
				EditorGUI.LabelField(
					new Rect(rect.x, rect.y, 160, EditorGUIUtility.singleLineHeight),
					"No params!");
			}

			EditorGUI.PropertyField(
				new Rect(rect.x + 160, rect.y, rect.width - 160 - 55, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("effectType"), GUIContent.none);
			EditorGUI.PropertyField(
				new Rect(rect.x + rect.width - 50, rect.y, 50, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("value"), GUIContent.none);
		};

		changersList.drawHeaderCallback = (Rect rect) => 
		{
			EditorGUI.LabelField(rect, "effects");
		};

	}
}
