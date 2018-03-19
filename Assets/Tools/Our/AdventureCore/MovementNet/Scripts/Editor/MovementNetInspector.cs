using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(MovementNet))]
public class MovementNetInspector : Editor
{
	private MovementNet net;
	private List<NetNode> selectedNodes = new List<NetNode>();
	private bool shiftPessed = false;
	private GameObject preview;
	private SpriteRenderer previewRenderer;

	private NetNode p1, p2;

	private void OnEnable()
	{
		preview = new GameObject ();
		preview.transform.localScale = Vector3.one;
		previewRenderer = preview.AddComponent<SpriteRenderer> ();
		preview.SetActive (false);

		net = (MovementNet)target;
		foreach (NetNode nn in net.nodes)
		{
			net.RecalculatePathesForNode(nn);
		}

		if (net.testTexture) 
		{
			previewRenderer.sprite = net.testTexture;
		}
	}

	private void OnDisable()
	{
		DestroyImmediate (preview);
	}

	public override void OnInspectorGUI()
	{
        Undo.RecordObject(net, "movement net");

		float dist = EditorGUILayout.Slider("connection disctance", net.minDistance, 0, 10);

		net.minSize = EditorGUILayout.Slider("minAgentSize", net.minSize, 0, 1);

		net.maxSize = EditorGUILayout.Slider("maxAgentSize", net.maxSize, 0, 2);

		if (dist!=net.minDistance)
		{
			net.minDistance = dist;
			foreach (NetNode nn in net.nodes)
			{
				net.RecalculatePathesForNode(nn);
			}
			SceneView.RepaintAll();
		}

		Sprite newTexture = (Sprite)EditorGUILayout.ObjectField (net.testTexture, typeof(Sprite), GUILayout.Width(200));
		if(net.testTexture!=newTexture)
		{
			net.testTexture = newTexture;
			if (newTexture) 
			{
				previewRenderer.sprite = newTexture;
			}
		}
	}

	void OnSceneGUI()
	{

		shiftPessed = Event.current.shift;
        Undo.RecordObject(net, "movement net");

	
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.D && shiftPessed) {
				if (selectedNodes.Count > 0) {
					foreach (NetNode node in selectedNodes) {
						net.RemoveNode (node);
						net.nodes.Remove (node);
					}
					SceneView.RepaintAll ();
				}
			}


			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.W && shiftPessed) {
				if (selectedNodes.Count > 0) {
				
					ChangeScale (0.1f, selectedNodes);
					SceneView.RepaintAll ();
				}
			}

			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.S && shiftPessed) {
				if (selectedNodes.Count > 0) {
					ChangeScale (-0.1f, selectedNodes);
					SceneView.RepaintAll ();
				}
			}

			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.N && shiftPessed) {  
				Vector3 mousePosition = Event.current.mousePosition;
				float mult = EditorGUIUtility.pixelsPerPoint;
				mousePosition.y = Camera.current.pixelHeight - mousePosition.y * mult;
				mousePosition.x *= mult;    
				Ray ray = Camera.current.ScreenPointToRay (mousePosition);
				Plane plane = new Plane ((Camera.current.transform.position - net.transform.position).normalized, net.transform.position);     
				float enter = 0;
				if (plane.Raycast (ray, out enter)) {
					Debug.Log ("!!");
					Vector3 hitPoint = ray.GetPoint (enter);
					//Move your cube GameObject to the point where you clicked
					Vector3 position = net.transform.InverseTransformPoint (hitPoint);

					NetNode newNode = new NetNode ();
					newNode.position = position;
					net.nodes.Add (newNode);
					net.RecalculatePathesForNode (newNode);
					Repaint ();
					SceneView.RepaintAll ();
				} 
			}

			foreach(NetNode node in net.nodes)
			{
				if (node.nodeName!=string.Empty)
				{
					Handles.Label(net.transform.TransformPoint(node.position),node.nodeName.ToString());
				}
			}

		foreach (NetNode node in net.nodes)
		{
			Handles.color = Color.black;
			float size = HandleUtility.GetHandleSize(node.position + net.transform.position)/5;
			float actualSize = size*node.scale;

			if (selectedNodes.Contains(node))
			{
				actualSize *= 1.8f;
				Handles.color = Color.green;

			}

			if (node.nodeName!=string.Empty)
			{
				Handles.color = Color.magenta;
			}

			MyHandles.DragHandleResult result;

			Vector3 np = net.transform.InverseTransformPoint(MyHandles.DragHandle(net.transform.TransformPoint(node.position),  actualSize, Handles.SphereCap, Color.green, out result));

			if (node.position!=np)
			{
				if(!selectedNodes.Contains(node))
				{
					selectedNodes.Clear ();
					selectedNodes.Add (node);
				}
				Vector3 dif = np - node.position;
				if (selectedNodes.Count == 0) 
				{
					selectedNodes.Add (node);
				}
				foreach (NetNode n in selectedNodes) {
					n.position += dif;
				}
				net.RecalculatePathesForNode(node);
			}



			switch (result)
			{
			case MyHandles.DragHandleResult.LMBClick:
				if (!shiftPessed) {
					selectedNodes.Clear ();
					selectedNodes.Add (node);
				} else {
					if (selectedNodes.Contains (node)) 
					{
						selectedNodes.Remove (node);
					} else {
						selectedNodes.Add (node);
					}
				}
				Repaint ();
				SceneView.RepaintAll();
				break;
			}


			Handles.color = Color.green;
			foreach (NetNode node2 in net.nodes)
			{


				if (node.Path(node2)<Mathf.Infinity)
				{
					if((node == p1 && node2 == p2)||(node == p2 && node2 == p1))
					{
						Handles.color = Color.red;
					}
					else
					{
						Handles.color = Color.green;
					}
					Handles.DrawLine(net.transform.TransformPoint(node.position), net.transform.TransformPoint(node2.position));
				}
			}
			Handles.color = Color.black;
		}


        Handles.BeginGUI();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+", GUILayout.Width(30)))
        {
            NetNode newNode = new NetNode();
            net.nodes.Add(newNode);
            net.RecalculatePathesForNode(newNode);
            Repaint();
            SceneView.RepaintAll();
        }

		if (selectedNodes.Count>0)
        {
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
				foreach (NetNode node in selectedNodes) {
					net.RemoveNode (node);
					net.nodes.Remove (node);
				}
                SceneView.RepaintAll();
            }

			string previousName = "-";
			if(selectedNodes.Count==1)
			{
				previousName = selectedNodes [0].nodeName;
			}
			string newName = EditorGUILayout.TextField(previousName, GUILayout.Width(150));
            
			if (selectedNodes.Count == 1) {
				if (newName != selectedNodes [0].nodeName) {
					selectedNodes [0].nodeName = newName;
					SceneView.RepaintAll ();
				}
			} else 
			{
				if (newName != "-") 
				{
					foreach (NetNode node in selectedNodes) 
					{
						node.nodeName = newName;
					}
					SceneView.RepaintAll ();
				}
			}

			float sliderValue = selectedNodes [0].scale;
			if(selectedNodes.Count>0)
			{
				if(selectedNodes.Select(n=>n.scale).Distinct().Count()!=selectedNodes.Count)
				{
					sliderValue = 0;
					foreach(NetNode nn in selectedNodes)
					{
						sliderValue += nn.scale;
					}
					sliderValue /= selectedNodes.Count;
				}
			}

			float newSlidervale = EditorGUILayout.FloatField(sliderValue, GUILayout.Width(50));

			newSlidervale = Mathf.Clamp (newSlidervale, 0, 3);

			if(sliderValue!=newSlidervale)
			{
				foreach(NetNode nn in selectedNodes)
				{
					nn.scale = newSlidervale;
				}
			}

        }
		

        EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal ();
		net.testTexture = (Sprite)EditorGUILayout.ObjectField (net.testTexture, typeof(Sprite), false, GUILayout.Width(200));
		bool activePreview = EditorGUILayout.Toggle (preview.activeInHierarchy);
		preview.SetActive (activePreview);
		EditorGUILayout.EndHorizontal ();
		SceneView.RepaintAll ();
        Handles.EndGUI();

		if (net.nodes.Count > 1 && activePreview) {
			Vector3 mousePoint = Vector3.zero;
			Vector3 mousePosition = Event.current.mousePosition;
			float mult = EditorGUIUtility.pixelsPerPoint;
			mousePosition.y = Camera.current.pixelHeight - mousePosition.y * mult;
			mousePosition.x *= mult;    
			Ray ray = Camera.current.ScreenPointToRay (mousePosition);
			Plane plane = new Plane ((Camera.current.transform.position - net.transform.position).normalized, net.transform.position);     

			float enter = 0;

			if (plane.Raycast (ray, out enter)) {
				Vector3 hitPoint = ray.GetPoint (enter);
				//Move your cube GameObject to the point where you clicked
				mousePoint = net.transform.InverseTransformPoint (hitPoint);
			}

			List<NetNode> nearestNodes = net.nodes.OrderBy (n => Vector3.Distance (n.position, mousePoint)).ToList();

			float scale = net.nodes [0].scale;
			if(activePreview && net.testTexture && net.nodes.Count>0)
			{
				preview.transform.position = mousePoint;
				preview.transform.localScale = scale*Vector3.one;
			}
			p1 = nearestNodes [0];
			p2 = nearestNodes [1];
			scale = 1;
			float size = HandleUtility.GetHandleSize (p1.position + net.transform.position);
			MyHandles.DragHandleResult result;
			Handles.color = Color.red;
			MyHandles.DragHandle (mousePoint, scale*size, Handles.SphereCap, Color.red, out result);
		}
	}

	public void ChangeScale(float mult, List<NetNode> nodes)
	{
		Vector3 average = Vector3.zero;
		foreach(NetNode nn in nodes)
		{
			average += nn.position;
		}
		average /= nodes.Count ();

		foreach(NetNode nn in net.nodes)
		{
			float dist = Mathf.Pow(Vector3.Distance (average, nn.position), 3);

			float val = 0;
			if (selectedNodes.Contains (nn)) {
				val = mult;
			} else 
			{
				val = mult / dist;
			}	
				
			if(val>0 && nn.scale>=3)
			{
				return;
			}


			if(val<0 && nn.scale<=0)
			{
				return;
			}

			nn.scale += val;
		}		
	}
}
