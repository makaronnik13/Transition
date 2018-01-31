using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

public class NodeEditor : EditorWindow
{
    private static PersonDialogs editingDialog;
    private static NarrativeNode editingNode;

	private HashSet<Symbol> learnedSymbols = new HashSet<Symbol> ();
	private Vector2 screenDelta = Vector2.zero;

    public static void ShowNode(NarrativeNode target)
    {
        editingNode = target;
    }

    private Vector2 lastMousePosition;
	private NarrativePath selectedPath;
	private List<KeyValuePair<NarrativeNode, GUIDraggableObject>> statesPositions = new List<KeyValuePair<NarrativeNode, GUIDraggableObject>> ();
	private bool lineType;

	private List<KeyValuePair<NarrativeNode, GUIDraggableObject>> StatesPositions {
		get {
			if (statesPositions.Count == 0) {
				int i = 0;

                statesPositions.Clear();

                foreach (NarrativeNode state in DialogNodes()) {
					KeyValuePair<NarrativeNode, GUIDraggableObject> kvp = new KeyValuePair<NarrativeNode, GUIDraggableObject> (state, new GUIDraggableObject (new Vector2 (state.X, state.Y)));
					statesPositions.Add (kvp);
					kvp.Value.onDrag += kvp.Key.Drag;
					i++;
				}
			}
			return statesPositions;
		}
	}

	private static Texture2D backgroundTexture;

    private static Texture2D BackgroundTexture {
		get
        {
			if (backgroundTexture == null) {
				backgroundTexture = (Texture2D)Resources.Load ("Sprites/background") as Texture2D;
				backgroundTexture.wrapMode = TextureWrapMode.Repeat;
			}
			return backgroundTexture;
		}
	}

	public  List<NarrativeNode> DialogNodes ()
	{   
		return editingDialog.nodes.ToList();
	}


	public static NodeEditor Init (PersonDialogs dialog)
	{
        editingDialog = dialog;
		NodeEditor window = (NodeEditor)EditorWindow.GetWindow<NodeEditor> ("Node editor", true, new Type[3] {
			typeof(Animator),
			typeof(Console),
			typeof(SceneView)
		});

		window.minSize = new Vector2 (600, 400);
		window.ShowAuxWindow ();
		return window;
	}

	private void OnDisable ()
	{
			EditorUtility.SetDirty (editingDialog);
	}

	private void OnGUI ()
	{
		Event currentEvent = Event.current;

        DraggingEvent();

        ContextMenuEvent(0);

		DrawChainsWindow ();
	}

    private void ContextMenuEvent(int v)
    {      
        Event currentEvent = Event.current;
        if (currentEvent.button == 1)
        {
            if (currentEvent.type == EventType.MouseDown)
            {
                GenericMenu menu = new GenericMenu();

                switch (v)
                {
                    default:

                        menu.AddItem(new GUIContent("New node"), false, () =>
                        {
                            CreateNewNode(new Vector2(currentEvent.mousePosition.x - screenDelta.x, currentEvent.mousePosition.y - screenDelta.y));
                        });
                        break;
                }
                menu.ShowAsContext();
            }
        }
    }

    private NarrativeNode CreateNewNode(Vector2 position)
    {

        Debug.Log("cnn");
        NarrativeNode state = (NarrativeNode)ScriptableObjectUtility.CreateAsset<NarrativeNode>(editingDialog);
        state.X = position.x;
        state.Y = position.y;   
        editingDialog.nodes.Add(state);
        Repaint();
        return state;
    }

    private void ClickEvent(int v, Rect rect, UnityEngine.Object clickedObject = null)
    {
        Event currentEvent = Event.current;
        if (rect.Contains(currentEvent.mousePosition) && currentEvent.button == 0)
        {
            if (currentEvent.type == EventType.MouseDown)
            {
                switch (v)
                {
                    case 1:   
                            Selection.activeObject = clickedObject;

                        editingDialog.nodes.Remove((NarrativeNode)clickedObject);
                        editingDialog.nodes.Add((NarrativeNode)clickedObject);
                        Repaint();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void DraggingEvent()
    {
        Event currentEvent = Event.current;
        if (currentEvent.button == 2)
        {
            if (currentEvent.type == EventType.MouseDown)
            {
                lastMousePosition = currentEvent.mousePosition;
            }

            if (currentEvent.type == EventType.MouseDrag)
            {

                Vector2 mouseMovementDifference = (currentEvent.mousePosition - lastMousePosition);

                screenDelta += new Vector2(mouseMovementDifference.x, mouseMovementDifference.y);

                lastMousePosition = currentEvent.mousePosition;
                currentEvent.Use();
            }
        }
    }

    private void DrawCreatingLine()
	{
		if (selectedPath != null)
		{

			Rect start = new Rect();
			foreach (KeyValuePair<NarrativeNode, GUIDraggableObject> p in StatesPositions)
			{
				float s = 70;
				int i = 0;
				foreach (NarrativePath c in p.Key.pathes)
				{
					float offset = s * i / (p.Key.pathes.Count() - 1);
					if (p.Key.pathes.Count() == 1)
					{
						offset = 100f / 2 - 5;
					}
					if (c == selectedPath)
					{         
						start = new Rect(p.Value.Position.x + screenDelta.x - 100f / 2 + 15f + offset, p.Value.Position.y + screenDelta.y, 100.0f, 115.0f);
					}

					i++;
				}
			}
			Handles.BeginGUI();

			float w = 2;
			Color color = Color.white;

			if(!lineType)
			{
				w = 1;
				color = Color.gray;
			}

			DrawNodeCurve(start.center, Event.current.mousePosition, color, w);
			Handles.EndGUI();
		}


	}
	
    /*
	private HashSet<Symbol> LearnedSymbols (Transmission tr, List<Transmission> visitedNodes)
	{
		HashSet<Symbol> learnedSymbols = new HashSet<Symbol> ();

		if (visitedNodes.Contains (tr)) {
			return learnedSymbols;
		}

		foreach (Symbol s in tr.content) {
			learnedSymbols.Add (s);
		}



		visitedNodes.Add (tr);
		foreach (Transmission tra in DialogNodes()) {
			foreach (Choice c in tra.choices) {
				if (c.nextTransmission == tr) {
					foreach (Symbol oldLearnedSymbol in LearnedSymbols(tra, visitedNodes)) {
						learnedSymbols.Add (oldLearnedSymbol);
					}

				}
			}
		}

		return learnedSymbols;
	}
    */
    /*
	private HashSet<Symbol> LearnedSymbolsWithoutCurrent (Transmission tr)
	{
		HashSet<Symbol> learnedSymbols = new HashSet<Symbol> ();


		foreach (Transmission tra in DialogNodes()) {
			foreach (Choice c in tra.choices) {
				if (c.nextTransmission == tr ) {
					foreach (Symbol oldLearnedSymbol in LearnedSymbols(tra, new List<Transmission>())) {
						learnedSymbols.Add (oldLearnedSymbol);
					}

				}
			}
		}

		return learnedSymbols;
	}
    */
  

    
    private void DrawChainsWindow ()
	{
		Rect fieldRect = new Rect (0, 0, position.width, position.height);
		GUI.DrawTextureWithTexCoords (fieldRect, BackgroundTexture, new Rect (0, 0, fieldRect.width / BackgroundTexture.width, fieldRect.height / BackgroundTexture.height));
		DrawPathes ();
		BeginWindows ();


        for (int i = 0; i <= StatesPositions.Count - 1; i++)
        {
            DrawStateBox (StatesPositions [i]);
		}
		EndWindows ();
	}

    private void DrawStateBox(KeyValuePair<NarrativeNode, GUIDraggableObject> state)
    {
        GUI.backgroundColor = Color.gray * 0.7f;

        if (Selection.activeObject == state.Key) {
            GUI.backgroundColor = GUI.backgroundColor * 1.7f;
        }

        float height = 20;

        if (Selection.activeObject == state.Key)
        {
            if (state.Key.Description == "")
            {
                height = 20;
            }
            else
            {
                height = 40 + EditorGUIUtility.singleLineHeight * (-1 + state.Key.Description.Split('\n').Length);
            }
        }

        Rect drawRect = new Rect(state.Value.Position.x + screenDelta.x, state.Value.Position.y + screenDelta.y, 150, height);//, dragRect;

        if (Event.current.type == EventType.MouseUp && drawRect.Contains(Event.current.mousePosition) && selectedPath != null)
        {
            if (Event.current.button == 1)
            {
                selectedPath.node = state.Key;
                selectedPath = null;
            }
        }

        ClickEvent(1, drawRect, state.Key);

        Event currentEvent = Event.current;

        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 1 && drawRect.Contains(currentEvent.mousePosition)) {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("AddChild"), false, () => {

                NarrativeNode newNode = CreateNewNode(new Vector2(currentEvent.mousePosition.x - screenDelta.x, currentEvent.mousePosition.y - screenDelta.y));
                state.Key.pathes.Add(new NarrativePath(newNode));
            });

            menu.AddItem(new GUIContent("DeleteNode"), false, () => {

                foreach (NarrativeNode tr in DialogNodes())
                {
                    foreach (NarrativePath c in tr.pathes)
                    {
                        if (c.node == state.Key)
                        {
                            c.node = null;
                        }
                    }
                }

                StatesPositions.Remove(StatesPositions.First(s => s.Key == state.Key));
                editingDialog.nodes.Remove(state.Key);
                DestroyImmediate(state.Key, true);
                AssetDatabase.SaveAssets();
                //AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(state.Key));
                Repaint();
                return;
            });
            menu.ShowAsContext();
        }



        GUILayout.BeginArea(drawRect, GUI.skin.GetStyle("Box"));
        GUILayout.BeginVertical();

        EditorGUILayout.LabelField(state.Key.name, EditorStyles.boldLabel);
        if (Selection.activeObject == state.Key)
        {
            try
            {
                GUI.color = Color.black * 0.6f;
                //EditorGUILayout.LabelField("");
                EditorGUILayout.LabelField(state.Key.Description, GUILayout.Height(EditorGUIUtility.singleLineHeight * (1 + state.Key.Description.Split('\n').Length)));
                GUI.color = Color.white;
                Repaint();
            }
            catch
            {

            }
        }
        
        /*
		foreach (Symbol s in state.Key.content) {

			GUIContent content = new GUIContent ();

			content.image = new Texture ();

			if (s.image) {
				content.image = s.image.texture;
			}
				
			try {
				EditorGUILayout.LabelField (content, GUILayout.Width (35), GUILayout.Height (35));
			} catch {
			}
		}*/


		GUILayout.EndVertical ();

		GUILayout.EndArea ();


		if (Selection.activeObject == state.Key) {
			state.Value.Drag (drawRect);
			Repaint ();
		}

	
		Repaint ();

		GUI.backgroundColor = Color.white;

	}

    private void DrawPathes ()
	{
		foreach (KeyValuePair<NarrativeNode, GUIDraggableObject> state in StatesPositions)
        {	
			Vector2 start = new Vector2 (state.Value.Position.x, state.Value.Position.y);

            int i = 0;

            foreach(NarrativePath np in state.Key.pathes)
            {
                if (np.node==null)
                {
                    continue;
                }
                Vector2 end = new Vector2(np.node.X, np.node.Y);
                DrawNodeCurve(screenDelta + start+Vector2.up*20+Vector2.right*(5 + i*(150/state.Key.pathes.Count)), screenDelta + end+Vector2.right*150/2, Color.white, 2);
                i++;
            }
		}
	}

	private void DrawNodeCurve (Vector2 startPos, Vector2 endPos, Color c, float width)
	{
		float force = 1f;
		float distanceY = Mathf.Abs (startPos.y - endPos.y);
		float distanceX = Mathf.Abs (startPos.x - endPos.x);
		Vector3 middlePoint = (startPos + endPos) / 2;

		Vector3 startTan1 = startPos;
		Vector3 endTan2 = endPos;
		Vector3 startTan2 = middlePoint;
		Vector3 endTan1 = middlePoint;

		if (startPos.y > endPos.y) {
			startTan1 -= Vector3.down * 150;
			endTan2 -= Vector3.up * 150;
			if (startPos.y > endPos.y) {
				endTan1 += Vector3.up * Mathf.Max (distanceY, 50);
				startTan2 -= Vector3.up * Mathf.Max (distanceY, 50);
			} else {
				endTan1 += Vector3.down * Mathf.Max (distanceY, 50);
				startTan2 -= Vector3.down * Mathf.Max (distanceY, 50);
			}
		} else {
			startTan1 -= distanceY * Vector3.down / force / 2;
			endTan2 -= distanceY * Vector3.up / force / 2;
			if (startPos.x > endPos.x) {
				endTan1 += distanceX * Vector3.right / force / 2;
				startTan2 -= distanceX * Vector3.right / force / 2;
			} else {
				endTan1 += distanceX * Vector3.left / force / 2;
				startTan2 -= distanceX * Vector3.left / force / 2;
			}
		}

		Color shadowCol = new Color (0, 0, 0, 0.06f);

		// Draw a shadow
		for (int i = 0; i < 2; i++) {
			Handles.DrawBezier (startPos, middlePoint, startTan1, endTan1, shadowCol, null, (i + 1) * 7 * width);
		}
		Handles.DrawBezier (startPos, middlePoint, startTan1, endTan1, c, null, 3 * width);

		for (int i = 0; i < 2; i++) {
			Handles.DrawBezier (middlePoint, endPos, startTan2, endTan2, shadowCol, null, (i + 1) * 7 * width);
		}
		Handles.DrawBezier (middlePoint, endPos, startTan2, endTan2, c, null, 3 * width);
	} 
}
