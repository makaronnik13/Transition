using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

public class NodeEditor : EditorWindow
{
    private static PersonDialogs editingDialog;
    private static NarrativeNode editingNode = null;
	private HashSet<Symbol> learnedSymbols = new HashSet<Symbol> ();
	private Vector2 screenDelta = Vector2.zero;
    private NarrativeNode savedPathNode = null;
    public static void ShowNode(NarrativeNode target)
    {
        editingNode = target;
    }
    private Vector2 lastMousePosition;
	private NarrativePath selectedPath;
	private List<KeyValuePair<NarrativeNode, GUIDraggableObject>> statesPositions = new List<KeyValuePair<NarrativeNode, GUIDraggableObject>> ();
	private List<KeyValuePair<NarrativeNode, GUIDraggableObject>> StatesPositions {
		get {

			if (statesPositions.Count == 0) {
                UpdateStatesPositions();
			}
			return statesPositions;
		}
	}

    private List<KeyValuePair<DialogNode, GUIDraggableObject>> narrativeNodesPositionss = new List<KeyValuePair<DialogNode, GUIDraggableObject>>();
    private List<KeyValuePair<DialogNode, GUIDraggableObject>> NarrativeNodesPositions
    {
        get
        {

            if (statesPositions.Count == 0)
            {
                UpdateNarrativeNodesPositions();
            }
            return narrativeNodesPositionss;
        }
    }

    private void UpdateStatesPositions()
    {
        int i = 0;

        statesPositions.Clear();

        foreach (NarrativeNode state in DialogNodes())
        {
            KeyValuePair<NarrativeNode, GUIDraggableObject> kvp = new KeyValuePair<NarrativeNode, GUIDraggableObject>(state, new GUIDraggableObject(new Vector2(state.X, state.Y)));
            statesPositions.Add(kvp);
            kvp.Value.onDrag += kvp.Key.Drag;
            i++;
        }
    }
    private void UpdateNarrativeNodesPositions()
    {
        int i = 0;

        narrativeNodesPositionss.Clear();

        foreach (DialogNode state in editingNode.dialogStates)
        {
            KeyValuePair<DialogNode, GUIDraggableObject> kvp = new KeyValuePair<DialogNode, GUIDraggableObject>(state, new GUIDraggableObject(new Vector2(state.X, state.Y)));
            narrativeNodesPositionss.Add(kvp);
            kvp.Value.onDrag += kvp.Key.Drag;
            i++;
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
        if (editingDialog==null)
        {
            return new List<NarrativeNode>();
        }
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

        if (!editingNode)
        {
            DrawChainsWindow();
        }
        else
        {
            DrawNodeWindow();
        }

        if (currentEvent.type == EventType.MouseDown && currentEvent.button != 2)
        {
            CancelMakingPath();
        }

        DrawTopPanel();
    }

    private void DrawNodeWindow()
    {
        Rect fieldRect = new Rect(0, 0, position.width, position.height);
        GUI.DrawTextureWithTexCoords(fieldRect, BackgroundTexture, new Rect(0, 0, fieldRect.width / BackgroundTexture.width, fieldRect.height / BackgroundTexture.height));
        DrawNarrativePathes();
        BeginWindows();
        for (int i = 0; i <= NarrativeNodesPositions.Count - 1; i++)
        {
            DrawDialogBox(NarrativeNodesPositions[i]);
        }
        EndWindows();
    }

    private void DrawDialogBox(KeyValuePair<DialogNode, GUIDraggableObject> state)
    {
        GUI.backgroundColor = Color.gray * 0.7f;

        if (Selection.activeObject == state.Key)
        {
            GUI.backgroundColor = GUI.backgroundColor * 1.7f;
        }

        float height = 20;

        if (Selection.activeObject == state.Key)
        {
            if (state.Key.replic == "")
            {
                height = 20;
            }
            else
            {
                height = 40 + EditorGUIUtility.singleLineHeight * (-1 + state.Key.replic.Split('\n').Length);
            }
        }

        Rect drawRect = new Rect(state.Value.Position.x + screenDelta.x, state.Value.Position.y + screenDelta.y, 150, height);//, dragRect;

        ClickEvent(1, drawRect, state.Key);

        Event currentEvent = Event.current;

        /*
        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 1 && drawRect.Contains(currentEvent.mousePosition))
        {
            CancelMakingPath();
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

            menu.AddItem(new GUIContent("AddPath"), false, () =>
            {
                NarrativePath np = new NarrativePath(null);
                selectedPath = np;
                state.Key.pathes.Add(np);

            });
            menu.ShowAsContext();
        }


    */

        GUILayout.BeginArea(drawRect, GUI.skin.GetStyle("Box"));
        GUILayout.BeginVertical();

        EditorGUILayout.LabelField(state.Key.name, EditorStyles.boldLabel);
        if (Selection.activeObject == state.Key)
        {
            try
            {
                GUI.color = Color.black * 0.6f;
                //EditorGUILayout.LabelField("");
                EditorGUILayout.LabelField(state.Key.replic, GUILayout.Height(EditorGUIUtility.singleLineHeight * (1 + state.Key.replic.Split('\n').Length)));
                GUI.color = Color.white;
                Repaint();
            }
            catch
            {

            }
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
        if (Selection.activeObject == state.Key)
        {
            state.Value.Drag(drawRect);
            Repaint();
        }
        Repaint();
        GUI.backgroundColor = Color.white;
    }

    private void DrawNarrativePathes()
    {
       // throw new NotImplementedException();
    }

    private void DrawTopPanel()
    {
        Rect fieldRect = new Rect(0, 0, position.width, 20);
        GUI.color = Color.gray;
        GUI.Box(fieldRect, GUIContent.none);
        GUI.color = Color.white;
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.normal.background = (Texture2D)Resources.Load("Sprites/topButton") as Texture2D;
        //buttonStyle.hover.background = (Texture2D)Resources.Load("Sprites/topButton") as Texture2D;
        //buttonStyle.active.background = (Texture2D)Resources.Load("Sprites/topButton") as Texture2D;

        if (editingNode)
        { 
        if (GUI.Button(new Rect(80, 0, 85, 18), editingNode.name, buttonStyle))
        {
            
        }
        }

        if (GUI.Button(new Rect(2, 0, 85, 18), "base layer", buttonStyle))
        {
            editingNode = null;
        }
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
        NarrativeNode state = (NarrativeNode)ScriptableObjectUtility.CreateAsset<NarrativeNode>(editingDialog);
        state.X = position.x;
        state.Y = position.y;   
        editingDialog.nodes.Add(state);
        UpdateStatesPositions();
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
                        SetPathEnd((NarrativeNode)clickedObject);
                        Repaint();
                        break;
                    default:
                        break;
                }
            }
        }
    }
    private void SetPathEnd(NarrativeNode node)
    {
        if (selectedPath!=null)
        {
            selectedPath.node = node;
            selectedPath = null;
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

        ClickEvent(1, drawRect, state.Key);

        Event currentEvent = Event.current;

        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 1 && drawRect.Contains(currentEvent.mousePosition))
        {
            CancelMakingPath();
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

            menu.AddItem(new GUIContent("AddPath"), false, () => 
            {
                NarrativePath np = new NarrativePath(null);
                selectedPath = np;
                state.Key.pathes.Add(np);
                
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
		GUILayout.EndVertical ();
		GUILayout.EndArea ();
		if (Selection.activeObject == state.Key) {
			state.Value.Drag (drawRect);
			Repaint ();
		}
		Repaint ();
		GUI.backgroundColor = Color.white;
	}
    private void CancelMakingPath()
    {
        foreach (NarrativeNode node in editingDialog.nodes)
        {
            foreach (NarrativePath p in node.pathes)
            {
                if (p.node == null)
                {
                    p.node = savedPathNode;
                    savedPathNode = null;
                }
            }
            node.pathes.RemoveAll(item => item.node == null);
        }
    }
    private void DrawPathes ()
	{
		foreach (KeyValuePair<NarrativeNode, GUIDraggableObject> state in StatesPositions)
        {	
			Vector2 start = new Vector2 (state.Value.Position.x, state.Value.Position.y);

            int i = 0;

            foreach(NarrativePath np in state.Key.pathes)
            {
                Vector2 end;
                if (np.node!=null)
                {
                    end = new Vector2(np.node.X, np.node.Y) + Vector2.right * 150 / 2+screenDelta;
                }
                else
                {
                    end = Event.current.mousePosition;
                }
                
                DrawNodeCurve(screenDelta + start+Vector2.up*20+Vector2.right*(5 + i*(150/state.Key.pathes.Count)), end, Color.white, 2, true);
                i++;
            }
		}
	}
	private void DrawNodeCurve (Vector2 startPos, Vector2 endPos, Color c, float width, bool withArrow = false)
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

        if (withArrow)
        {
            Handles.DrawAAPolyLine(5, new Vector3[] {endPos+ new Vector2(5, -10), endPos, endPos+new Vector2(-5,-10)});
        }
	} 
}
