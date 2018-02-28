using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Person))]
public class PersonInspector : Editor {

	private Person person;
	private bool showPathEvents = false;
	private bool showStateEvents = false;

	void OnEnable () 
	{
		person = (Person)target;
		ResetEvents ();
	}

	private void ResetEvents()
	{
		if(person.dialog)
		{
			foreach(DialogNode node in person.dialog.nodes)
			{
				foreach(DialogStateNode subNode in node.dialogState.nodes)
				{
					if(subNode.withEvent && person.nodeEvents.Find(ne=>ne.node==subNode)== null)
					{
						person.nodeEvents.Add (new StateEvent(subNode));
					}

					foreach(DialogStatePath path in subNode.pathes)
					{
						if(path.withEvent && person.pathEvents.Find(ne=>ne.path==path)== null)
						{
							person.pathEvents.Add (new PathEvent(path));
						}
					}
				}	
			}
		}
	}

	public override void OnInspectorGUI()
	{
		Dialog newDialog = (Dialog)EditorGUILayout.ObjectField ("Dialog: ",person.dialog, typeof(Dialog), false);


		if (newDialog != person.dialog)
		{
			person.dialog = newDialog;

			ResetEvents ();
		}

		if (person.dialog) {
			showStateEvents = EditorGUILayout.Foldout (showStateEvents, "State events");

			if (showStateEvents) {
				foreach (DialogNode node in newDialog.nodes) {
					foreach (DialogStateNode subNode in node.dialogState.nodes) {
						if (!subNode.withEvent) {
							continue;
						}
							
						StateEvent pair = person.nodeEvents.Find (ie => ie.node == subNode);

						EditorGUILayout.BeginHorizontal ();
						EditorGUILayout.LabelField (pair.node.name + "->", GUILayout.Width (80));
						serializedObject.Update ();
						EditorGUILayout.PropertyField (serializedObject.FindProperty ("nodeEvents").GetArrayElementAtIndex (person.nodeEvents.IndexOf (pair)).FindPropertyRelative ("activationEvent"));
						serializedObject.ApplyModifiedProperties ();
						EditorGUILayout.EndHorizontal ();
					}	
				}
			}

			showPathEvents = EditorGUILayout.Foldout (showPathEvents, "Path events");

			if (showPathEvents) {
				foreach (DialogNode node in newDialog.nodes) {
					foreach (DialogStateNode subNode in node.dialogState.nodes) {
						foreach (DialogStatePath path in subNode.pathes) {

							if (!path.withEvent) {
								continue;
							}

							PathEvent pair = person.pathEvents.Find (ie => ie.path == path);

							EditorGUILayout.BeginHorizontal ();
							EditorGUILayout.LabelField (pair.path.name + "->", GUILayout.Width (80));
							serializedObject.Update ();
							EditorGUILayout.PropertyField (serializedObject.FindProperty ("pathEvents").GetArrayElementAtIndex (person.pathEvents.IndexOf (pair)).FindPropertyRelative ("activationEvent"));
							serializedObject.ApplyModifiedProperties ();
							EditorGUILayout.EndHorizontal ();
						}
					}	
				}
			}
		}
	}
}
