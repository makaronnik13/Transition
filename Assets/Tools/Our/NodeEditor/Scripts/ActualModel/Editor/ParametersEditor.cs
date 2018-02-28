using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using GraphEditor;
using UnityEditorInternal;

public static class ParametersEditor 
{
	private static Vector2 scrollPosition;
	private static string searchString;
	private static ParamCollection collection;
	private static Dialog lastDialog;
	private static Rect lastRect;

	public static void Draw(Rect rect, Dialog dialog)
	{
		try
		{
		lastDialog = dialog;
		lastRect = rect;
		GUILayout.BeginArea(rect, GUI.skin.GetStyle("Box"));
		GUILayout.BeginVertical();

		if(dialog)
		{
			collection = dialog.paramCollection;
			dialog.paramCollection = (ParamCollection)EditorGUILayout.ObjectField (dialog.paramCollection, typeof(ParamCollection), false);
		}

		if(collection != null)
		{
			EditorGUILayout.BeginHorizontal ();
			searchString = EditorGUILayout.TextField (searchString, GUILayout.Width(160));
			if(GUILayout.Button("+", GUILayout.Width(EditorGUIUtility.singleLineHeight+3)))
			{
				AddParam ();
			}
			EditorGUILayout.EndHorizontal ();

			scrollPosition = GUILayout.BeginScrollView (scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);

			foreach(GameParameter gp in GameParameters)
			{
				EditorGUILayout.LabelField ("");
				Rect drawRect = GUILayoutUtility.GetLastRect ();
				
					Color color = Color.gray;
					if(Selection.activeObject == gp)
					{
						color*=1.5f;
					}

					EditorGUI.DrawRect (drawRect, color);
				string newName = EditorGUI.TextField (new Rect(drawRect.position+Vector2.right*20, drawRect.size*0.9f) , gp.name);

					if(GameParameters.Find(p=>p.name == newName && p!=gp))
					{
						newName = newName +"_"+Random.Range(0, 100);		
					}

				if(newName!= gp.name)
				{
						

					gp.name = newName;
					AssetDatabase.SaveAssets ();
				}

				if(Event.current.type == EventType.MouseDown && drawRect.Contains(Event.current.mousePosition))
				{
					if(Event.current.button == 1){
					GenericMenu menu = new GenericMenu ();

					menu.AddItem(new GUIContent("Delete"), false, () =>
						{

							RemoveParam(gp);
						});
					menu.ShowAsContext ();
						}
						if(Event.current.button == 0)
						{
							Selection.activeObject = gp;
							Draw (lastRect, lastDialog);
						}
				}
			}

			GUILayout.EndScrollView ();
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
		}
		catch
		{
			
		}
	}
		

	public static List<GameParameter> GameParameters
	{
		get
		{
			return collection.Parameters;
		}
	}

	private static List<GameParameter> SortedParameters()
	{
		if(searchString == "")
		{
			return GameParameters;
		}
		return GameParameters.Where (gp=>gp.name.Contains(searchString)).ToList();
	}

	private static void AddParam()
	{
		collection.Parameters.Add((GameParameter)ScriptableObjectUtility.CreateAsset<GameParameter>(collection));
		AssetDatabase.SaveAssets();
		Draw (lastRect, lastDialog);
	}

	private static void RemoveParam(GameParameter param)
	{
		collection.Parameters.Remove(param);
		UnityEngine.Object.DestroyImmediate(param, true);
		AssetDatabase.SaveAssets();
		Draw (lastRect, lastDialog);
	}
}
