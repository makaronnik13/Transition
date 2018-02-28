
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Linq;

[CustomEditor(typeof(DialogStatePath))]
public class DialogStatePathInspector : Editor
{
    private DialogStatePath state;
	private ReorderableList conditionsList;
	private ReorderableList changersList;

    void Awake()
    {
        state = (DialogStatePath)target;
		CreateConditionsList ();
		CreateChangersList ();
    }

    public override void OnInspectorGUI()
    {
        string newName = EditorGUILayout.TextField("Name: ", state.name);
        if (newName != state.name)
        {
            state.name = newName;
            AssetDatabase.SaveAssets();
        }

        if(!state.Start)
        {
            return;
        }


        if ( ((DialogStateNode)state.Start).nodeType != DialogStateNode.StateNodeType.enter) { 
			state.text = EditorGUILayout.TextField ("Text: ", state.text, GUILayout.Height (4 * EditorGUIUtility.singleLineHeight));
		}

		state.comentary = EditorGUILayout.TextField ("Comentary: ", state.comentary, GUILayout.Height (2 * EditorGUIUtility.singleLineHeight));

		EditorGUI.BeginDisabledGroup (((DialogStateNode)state.Start).nodeType == DialogStateNode.StateNodeType.enter);
		state.automatic = EditorGUILayout.Toggle ("Automatic", state.automatic);
		state.withEvent = EditorGUILayout.Toggle ("With event:", state.withEvent);
		serializedObject.Update();
		conditionsList.DoLayoutList();
		changersList.DoLayoutList ();
		serializedObject.ApplyModifiedProperties();
		EditorGUI.EndDisabledGroup ();

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

	private void CreateConditionsList()
	{
		conditionsList = new ReorderableList(serializedObject, 
			serializedObject.FindProperty("conditions"), 
			true, true, true, true);
		conditionsList.drawElementCallback =  
			(Rect rect, int index, bool isActive, bool isFocused) => {
			var element = conditionsList.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += 2;

			Dialog dialog = AssetDatabase.LoadAssetAtPath<Dialog>(AssetDatabase.GetAssetPath(state)); 
			if(dialog.paramCollection.Parameters.Count>0)
			{
				int paramIndex = 0;





				if(!dialog.paramCollection.Parameters.Contains(state.conditions[index].parameter))
				{
					state.conditions[index].parameter = dialog.paramCollection.Parameters[0];
				}


				paramIndex = EditorGUI.Popup(
					new Rect(rect.x, rect.y, 160, EditorGUIUtility.singleLineHeight),
					dialog.paramCollection.Parameters.IndexOf(state.conditions[index].parameter), dialog.paramCollection.Parameters.Select(p=>p.name).ToArray());


				state.conditions[index].parameter = dialog.paramCollection.Parameters[paramIndex];

			}
			else
			{
				state.conditions[index].parameter = null;
				EditorGUI.LabelField(
					new Rect(rect.x, rect.y, 160, EditorGUIUtility.singleLineHeight),
					"No params!");
			}

			EditorGUI.PropertyField(
				new Rect(rect.x + 160, rect.y, rect.width - 160 - 55, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("conditionType"), GUIContent.none);
			EditorGUI.PropertyField(
				new Rect(rect.x + rect.width - 50, rect.y, 50, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("value"), GUIContent.none);
		};

		conditionsList.drawHeaderCallback = (Rect rect) => 
		{
			EditorGUI.LabelField(rect, "conditions");
		};
	}
}
