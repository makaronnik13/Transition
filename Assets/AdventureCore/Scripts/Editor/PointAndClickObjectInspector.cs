using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(InteractableObject))]
public class PointAndClickObjectInspector : Editor
{
    private ReorderableList list;
    private InteractableObject item;

    private void OnEnable()
    {
        item = (InteractableObject)target;
        list = new ReorderableList(serializedObject,
               serializedObject.FindProperty("combinqations"),
               true, true, true, true);

        list.drawElementCallback =
    (Rect rect, int index, bool isActive, bool isFocused) => {
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;
        EditorGUI.PropertyField(
            new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("item"), GUIContent.none);
        EditorGUI.PropertyField(
            new Rect(rect.x, rect.y+ EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight*3),
            element.FindPropertyRelative("combinationDescription"), GUIContent.none);
        EditorGUI.PropertyField(
            new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight*4, rect.width/2-3, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("destroyItemAfterCombination"), new GUIContent("destroy item"));
        EditorGUI.PropertyField(
            new Rect(rect.x + rect.width/2+6, rect.y + EditorGUIUtility.singleLineHeight * 4, rect.width / 2 - 3, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("withEvent"), new GUIContent("with event"));
       
    };
        list.elementHeight = EditorGUIUtility.singleLineHeight * 5+5;
    }

    public override void OnInspectorGUI()
    {
        item.objectName = EditorGUILayout.TextField("Name", item.objectName);
        item.descripion = EditorGUILayout.TextArea("Description", item.descripion, GUILayout.Height(EditorGUIUtility.singleLineHeight*4));
        item.objType = (InteractableObject.InteractableObjectType)EditorGUILayout.EnumPopup("Type", item.objType);
        item.interactable = EditorGUILayout.Toggle("Interactable", item.interactable);
        if (item.objType == InteractableObject.InteractableObjectType.MoveTrigger)
        {
            item.sceneId = EditorGUILayout.IntField("Scene Id", item.sceneId);
        }

        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
