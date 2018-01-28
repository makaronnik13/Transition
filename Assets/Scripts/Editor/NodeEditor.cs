using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Sprites;

public class NodeEditor : EditorWindow
{
	private HashSet<Symbol> learnedSymbols = new HashSet<Symbol> ();
	private Symbol draggingSymbol;
	private Vector2 screenDelta = Vector2.zero;
	private Vector2 lastMousePosition;
	private Vector2 scrollPosition;
	private Choice selectedPath;
	private Transmission selectedTransmission;
	private List<KeyValuePair<Transmission, GUIDraggableObject>> statesPositions = new List<KeyValuePair<Transmission, GUIDraggableObject>> ();
	private bool lineType;

	private List<KeyValuePair<Transmission, GUIDraggableObject>> StatesPositions {
		get {
			if (statesPositions.Count == 0) {
				int i = 0;

				foreach (Transmission state in ProjectStates()) {
					KeyValuePair<Transmission, GUIDraggableObject> kvp = new KeyValuePair<Transmission, GUIDraggableObject> (state, new GUIDraggableObject (new Vector2 (state.X, state.Y)));
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
		get {
			if (backgroundTexture == null) {
				backgroundTexture = (Texture2D)Resources.Load ("Sprites/background") as Texture2D;
				backgroundTexture.wrapMode = TextureWrapMode.Repeat;
			}
			return backgroundTexture;
		}
	}

	public static List<Transmission> ProjectStates ()
	{
		List<Transmission> st = new List<Transmission> ();
		string[] guids = AssetDatabase.FindAssets ("t:Transmission");


		foreach (string s in guids) {
			string assetPath = AssetDatabase.GUIDToAssetPath (s);
			Transmission asset = AssetDatabase.LoadAssetAtPath (assetPath, typeof(Transmission)) as Transmission;
			st.Add (asset);
		}

		return st;
	}

	public static List<Symbol> Symbols ()
	{
		List<Symbol> st = new List<Symbol> ();
		string[] guids = AssetDatabase.FindAssets ("t:Symbol");


		foreach (string s in guids) {
			string assetPath = AssetDatabase.GUIDToAssetPath (s);
			Symbol asset = AssetDatabase.LoadAssetAtPath (assetPath, typeof(Symbol)) as Symbol;
			st.Add (asset);
		}
			
		return st;
	}

	[MenuItem ("Window/NodeEditor")]
	static NodeEditor Init ()
	{
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
		foreach (KeyValuePair<Transmission, GUIDraggableObject> kvp in StatesPositions) {
			kvp.Key.Drag (kvp.Value.Position);
			EditorUtility.SetDirty (kvp.Key);
		}
	}

	private void OnGUI ()
	{
		Event currentEvent = Event.current;
		if (currentEvent.button == 2) {
			if (currentEvent.type == EventType.MouseDown) {
				lastMousePosition = currentEvent.mousePosition;
			}

			if (currentEvent.type == EventType.MouseDrag) {

				Vector2 mouseMovementDifference = (currentEvent.mousePosition - lastMousePosition);

				screenDelta += new Vector2 (mouseMovementDifference.x, mouseMovementDifference.y);

				lastMousePosition = currentEvent.mousePosition;
				currentEvent.Use ();
			}
		}
			


		DrawChainsWindow ();
		DrawDictionary ();
		DrawDragging ();
		DrawCreatingLine();
	}

	private void DrawCreatingLine()
	{
		if (selectedPath != null)
		{

			Rect start = new Rect();
			foreach (KeyValuePair<Transmission, GUIDraggableObject> p in StatesPositions)
			{
				float s = 70;
				int i = 0;
				foreach (Choice c in p.Key.choices)
				{
					float offset = s * i / (p.Key.choices.Count() - 1);
					if (p.Key.choices.Count() == 1)
					{
						offset = 100f / 2 - 5;
					}
					if (c== selectedPath)
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


	private void DrawDragging()
	{
		if(draggingSymbol)
		{
			GUI.color = Color.white * 0.8f;
			Vector2 center = Event.current.mousePosition;
			GUIContent content = new GUIContent ();

			if (!draggingSymbol.image) {
				Texture2D texture = new Texture2D (1, 1, TextureFormat.ARGB32, false);
				texture.SetPixel (0, 0, Color.white);
				texture.Apply ();
				content.image = texture;
			}
			else
			{
				content.image = draggingSymbol.image.texture;
			}

			EditorGUI.LabelField (new Rect(center.x-15, center.y-15, 30 ,30 ), content);
			GUI.color = Color.white;
			if (Event.current.type == EventType.MouseUp && Event.current.button == 0) 
			{
				draggingSymbol = null;
			}
		}
	}


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
		foreach (Transmission tra in ProjectStates()) {
			foreach (Choice c in tra.choices) {
				if (c.nextTransmission == tr || c.addTransmissions.Contains (tr)) {
					foreach (Symbol oldLearnedSymbol in LearnedSymbols(tra, visitedNodes)) {
						learnedSymbols.Add (oldLearnedSymbol);
					}

				}
			}
		}

		return learnedSymbols;
	}

	private HashSet<Symbol> LearnedSymbolsWithoutCurrent (Transmission tr)
	{
		HashSet<Symbol> learnedSymbols = new HashSet<Symbol> ();


		foreach (Transmission tra in ProjectStates()) {
			foreach (Choice c in tra.choices) {
				if (c.nextTransmission == tr || c.addTransmissions.Contains (tr)) {
					foreach (Symbol oldLearnedSymbol in LearnedSymbols(tra, new List<Transmission>())) {
						learnedSymbols.Add (oldLearnedSymbol);
					}

				}
			}
		}

		return learnedSymbols;
	}

	private void DrawDictionary ()
	{
		EditorGUILayout.BeginVertical ();
		EditorGUILayout.BeginHorizontal (GUILayout.Height(20));
		if(GUILayout.Button("+tr"))
		{
			string assetPath = "Assets/";
			if(ProjectStates ().Count>0){
				assetPath = AssetDatabase.GetAssetPath (ProjectStates () [0]);
			}
			Transmission state = (Transmission)ScriptableObjectUtility.CreateAsset<Transmission> (assetPath);


			KeyValuePair<Transmission, GUIDraggableObject> kvp = new KeyValuePair<Transmission, GUIDraggableObject> (state, new GUIDraggableObject (new Vector2 (position.center.x - screenDelta.x, position.center.y - screenDelta.y)));
			statesPositions.Add (kvp);
			kvp.Value.onDrag += kvp.Key.Drag;
			Repaint ();
		}
			//Add transition
		if(GUILayout.Button("+sy"))
		{
			//add symbol
			string assetPath = "Assets/";
			if(Symbols().Count>0){
				assetPath = AssetDatabase.GetAssetPath (Symbols() [0]);
			}
			ScriptableObjectUtility.CreateAsset<Symbol> (assetPath);
			Repaint ();
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.LabelField ("", GUILayout.Height (position.height - 135));
		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition, GUIStyle.none, GUIStyle.none, GUILayout.Height (110), GUILayout.Width (position.width));
		EditorGUILayout.BeginHorizontal ();
		foreach (Symbol s in Symbols()) {

			GUI.color = Color.gray;



			if (selectedTransmission && learnedSymbols.Contains (s)) {
				GUI.color = Color.yellow;
			}


			if (selectedTransmission && selectedTransmission.content.Except (learnedSymbols).Contains (s)) {
				GUI.color = Color.green;
			}


			GUILayout.Box ("", GUILayout.Width (100), GUILayout.Height (100));

			GUI.color = Color.white;

	
			GUIContent content = new GUIContent ();
			if (s.image) {
				content.image = s.image.texture;
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && GUILayoutUtility.GetLastRect ().Contains (Event.current.mousePosition)) {
				Selection.activeObject = s;
			}

			if (Event.current.type == EventType.MouseDrag && Event.current.button == 0 && GUILayoutUtility.GetLastRect ().Contains (Event.current.mousePosition)) 
			{
				draggingSymbol = s;
			}

			EditorGUI.LabelField (GUILayoutUtility.GetLastRect (), content);
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndScrollView ();
		EditorGUILayout.EndVertical ();
	}


	private void DrawChainsWindow ()
	{
		Rect fieldRect = new Rect (0, 0, position.width, position.height);
		GUI.DrawTextureWithTexCoords (fieldRect, BackgroundTexture, new Rect (0, 0, fieldRect.width / BackgroundTexture.width, fieldRect.height / BackgroundTexture.height));
		DrawPathes ();
		BeginWindows ();

		for (int i = 0; i <= StatesPositions.Count - 1; i++) {

			DrawStateBox (StatesPositions [i]);
		}
		EndWindows ();
	}



	private void DrawStateBox (KeyValuePair<Transmission, GUIDraggableObject> state)
	{


		GUI.backgroundColor = Color.gray * 0.7f;


		if (Selection.activeObject == state.Key) {
			GUI.backgroundColor = GUI.backgroundColor * 1.7f;
		}


		Rect drawRect = new Rect (state.Value.Position.x + screenDelta.x, state.Value.Position.y + screenDelta.y, 35f * state.Key.content.Count () + 10, 45f);//, dragRect;


		if (Event.current.type == EventType.MouseUp && drawRect.Contains (Event.current.mousePosition) && selectedPath!=null) 
		{
			if(Event.current.button == 0 )
			{
				List<Transmission> old = selectedPath.addTransmissions.ToList ();
				old.Add (state.Key);
				selectedPath.addTransmissions = old.ToArray ();
				selectedPath = null;
			}
			if(Event.current.button == 1)
			{
				selectedPath.nextTransmission = state.Key;
				selectedPath = null;
			}
		}

		if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && drawRect.Contains (Event.current.mousePosition) && draggingSymbol) 
		{
			List<Symbol> old = state.Key.content.ToList ();
			old.Add (draggingSymbol);
			state.Key.content = old.ToArray ();
			draggingSymbol = null;
		}
		if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && drawRect.Contains (Event.current.mousePosition)) {
			Selection.activeObject = state.Key;
			selectedTransmission = state.Key;

			learnedSymbols = LearnedSymbolsWithoutCurrent (selectedTransmission);
			KeyValuePair<Transmission, GUIDraggableObject> kvp = StatesPositions.Find (k => k.Key == Selection.activeObject);
			StatesPositions.Remove (kvp);
			StatesPositions.Add (kvp);
			Repaint ();
		}
			
		Event currentEvent = Event.current;

		if (currentEvent.type == EventType.MouseDown && currentEvent.button == 1 && drawRect.Contains (currentEvent.mousePosition)) {
			GenericMenu menu = new GenericMenu ();
		
			menu.AddItem (new GUIContent ("AddNodeType1"), false, () => {
				Transmission newState = (Transmission)ScriptableObjectUtility.CreateAsset<Transmission> (AssetDatabase.GetAssetPath (ProjectStates () [0]));

				Choice c = new Choice ();
				c.nextTransmission = newState;
				List<Choice> oldChoices = state.Key.choices.ToList ();

				oldChoices.Add (c);
				state.Key.choices = oldChoices.ToArray ();
					
				KeyValuePair<Transmission, GUIDraggableObject> kvp = new KeyValuePair<Transmission, GUIDraggableObject> (newState, new GUIDraggableObject (new Vector2 (currentEvent.mousePosition.x - screenDelta.x, currentEvent.mousePosition.y - screenDelta.y)));
				statesPositions.Add (kvp);
				kvp.Value.onDrag += kvp.Key.Drag;
				Repaint ();
			});
				
			menu.AddItem (new GUIContent ("DeleteNode"), false, () => {

				foreach(Transmission tr in ProjectStates())
				{
					foreach(Choice c in tr.choices)
					{
						if(c.nextTransmission == state.Key)
						{
							c.nextTransmission = null;
						}

						if(c.addTransmissions.ToList().Contains(state.Key))
						{
							List<Transmission> old = c.addTransmissions.ToList();
							old.RemoveAll(t=>t==state.Key);
							c.addTransmissions = old.ToArray();
						}
					}
				}

				StatesPositions.Remove(StatesPositions.First(s=>s.Key==state.Key));
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(state.Key));
				Repaint();
			});
			menu.ShowAsContext ();
		}
				
		GUILayout.BeginArea (drawRect, GUI.skin.GetStyle ("Box"));
		GUILayout.BeginHorizontal ();

	
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
		}


		GUILayout.EndHorizontal ();

		GUILayout.EndArea ();


		if (Selection.activeObject == state.Key) {
			state.Value.Drag (drawRect);
			Repaint ();
		}

		state.Value.Drag (drawRect);
		Repaint ();

		GUI.backgroundColor = Color.white;

	}


	private void DrawPathes ()
	{
		foreach (KeyValuePair<Transmission, GUIDraggableObject> state in StatesPositions) {
			float width = state.Key.content.Count () * 35;
			Vector2 start = new Vector2 (state.Value.Position.x + 10, state.Value.Position.y + 35);

	
			int i = 0;
			foreach (Choice c in state.Key.choices) {

				if(c.nextTransmission)
				{
					KeyValuePair<Transmission, GUIDraggableObject> ending = StatesPositions.Find (k => k.Key == c.nextTransmission);
					Vector2 end = new Vector2 (5 + ending.Value.Position.x + ending.Key.content.Count () * 35f / 2, ending.Value.Position.y);
					DrawNodeCurve (screenDelta + start + Vector2.right * i * width / state.Key.choices.Count(), screenDelta + end, Color.white, 2);
				}
				i++;

				foreach(Transmission tr in c.addTransmissions)
				{
					KeyValuePair<Transmission, GUIDraggableObject> ending = StatesPositions.Find (k => k.Key == tr);
					Vector2 end = new Vector2 (5 + ending.Value.Position.x + ending.Key.content.Count () * 35f / 2, ending.Value.Position.y);
					DrawNodeCurve (screenDelta + start + Vector2.right * (i-1) * width / state.Key.choices.Count(), screenDelta + end, Color.gray, 1);

				}

				Rect startt = new Rect(screenDelta + start + Vector2.right * (i-1.5f) * width / state.Key.choices.Count() + 7.5f*Vector2.one, Vector2.one*30);

				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1);

				GUI.Label(startt, new GUIContent(Resources.Load("Sprites/button") as Texture2D));

				if (startt.Contains(Event.current.mousePosition))
				{

					if (Event.current.type == EventType.MouseDown)
					{
						if(Event.current.button == 0 || Event.current.button == 1)
						{

						if (selectedPath != c)
						{
							selectedPath = c;
							Repaint();
						}
						else
						{
							selectedPath = null;
							Repaint();
						}
							if(Event.current.button == 0)
							{
								lineType = false;
							}
							if(Event.current.button == 1)
							{
								lineType = true;	
							}	
						}
					}

				}

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
