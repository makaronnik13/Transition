using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

[CustomEditor(typeof(PointAndClickObject))]
public class PointAndClickObjectIngameInspector : Editor
{
    private bool showEvents = false;
    private PointAndClickObject item;
    private SerializedProperty activationEventProp;

    private void OnEnable()
    {
        item = (PointAndClickObject)target;
        if (item.objectAsset)
        {
            foreach (Combinations combo in item.objectAsset.combinqations)
            {
                ItemEvent pair = item.itemsEvents.Find(ie => ie.item == combo.item);
                if (pair == null)
                {
                    item.itemsEvents.Add(new ItemEvent(combo.item));
                }
            }

            List<ItemEvent> removingevents = new List<ItemEvent>();
            foreach (ItemEvent ie in item.itemsEvents)
            {
                if (item.objectAsset.combinqations.ToList().Find(c=>c.item == ie.item)==null)
                {
                    removingevents.Add(ie);
                }
            }

            foreach (ItemEvent ie in removingevents)
            {
                item.itemsEvents.Remove(ie);
            }

            activationEventProp = serializedObject.FindProperty("onActivation");
        }
    }

    public override void OnInspectorGUI()
    {
        InteractableObject  newItem = (InteractableObject)EditorGUILayout.ObjectField("Asset", item.objectAsset, typeof(InteractableObject), false);
        if (newItem != item.objectAsset)
        {
            item.objectAsset = newItem;
            item.itemsEvents = new List<ItemEvent>();
            if (item.objectAsset)
            {
                foreach (Combinations comb in item.objectAsset.combinqations)
                {
                    item.itemsEvents.Add(new ItemEvent(comb.item));
                }
            }
        }

        showEvents =  EditorGUILayout.Foldout(showEvents, "events");

        if (showEvents)
        {
            if (item.objectAsset.interactable)
            {
                serializedObject.Update();
                EditorGUILayout.PropertyField(activationEventProp);
                serializedObject.ApplyModifiedProperties();
            }


            foreach (Combinations combo in item.objectAsset.combinqations)
            {           
                if (!combo.withEvent)
                {
                    return;
                }

                ItemEvent pair = item.itemsEvents.Find(ie=>ie.item == combo.item);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(pair.item.itemName+"->", GUILayout.Width(80));
                serializedObject.Update();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("itemsEvents").GetArrayElementAtIndex(item.itemsEvents.IndexOf(pair)).FindPropertyRelative("activationEvent"));
                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndHorizontal();
            }
        }
        
    }
}
