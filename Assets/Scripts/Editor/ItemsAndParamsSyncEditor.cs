using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(ItemsAndParametersSync))]
public class ItemsAndParamsSyncEditor : Editor
{

	private ItemsAndParametersSync sync;
	private ReorderableList syncList;

	void  OnEnable()
	{
		sync = (ItemsAndParametersSync)target;
		syncList = new ReorderableList(serializedObject,
			serializedObject.FindProperty("syncList"),
			true, true, true, true);

		syncList.drawElementCallback =
			(Rect rect, int index, bool isActive, bool isFocused) => {
			var element = syncList.serializedProperty.GetArrayElementAtIndex(index);
			rect.y+=2;

			EditorGUI.PropertyField(
				new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("item"), GUIContent.none);
			EditorGUI.PropertyField(
				new Rect(rect.x, rect.y+ EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight*3),
				element.FindPropertyRelative("parameter"), GUIContent.none); 

		};

	}


	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		syncList.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
	}
}
