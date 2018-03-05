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
	private NetNode selectedNode;

	private void OnEnable()
	{
		net = (MovementNet)target;
		foreach (NetNode nn in net.nodes)
		{
			net.RecalculatePathesForNode(nn);
		}
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
	}

	void OnSceneGUI()
	{
        Undo.RecordObject(net, "movement net");

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.D && Event.current.shift)
		{
			if(selectedNode != null)
			{
				net.RemoveNode(selectedNode);
				net.nodes.Remove(selectedNode);
				SceneView.RepaintAll();
			}
		}


		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.N && Event.current.shift)
		{  
			Vector3 mousePosition = Event.current.mousePosition;
			float mult = EditorGUIUtility.pixelsPerPoint;
			mousePosition.y = Camera.current.pixelHeight - mousePosition.y * mult;
			mousePosition.x *= mult;    
			Ray ray = Camera.current.ScreenPointToRay(mousePosition);
			Plane plane = new Plane((Camera.current.transform.position-net.transform.position).normalized, net.transform.position);     
			float enter = 0;
			if (plane.Raycast(ray, out enter))
			{
				Debug.Log("!!");
				Vector3 hitPoint = ray.GetPoint(enter);
				//Move your cube GameObject to the point where you clicked
				Vector3 position = net.transform.InverseTransformPoint(hitPoint);

				NetNode newNode = new NetNode();
				newNode.position = position;
				net.nodes.Add(newNode);
				net.RecalculatePathesForNode(newNode);
				Repaint();
				SceneView.RepaintAll();
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
			float size = HandleUtility.GetHandleSize(node.position + net.transform.position)/10;
			float actualSize = size;

			if (selectedNode == node)
			{
				actualSize *= 1.2f;
				Handles.color = Color.green;

			}

			if (node.nodeName!=string.Empty)
			{
				actualSize *= 1.2f;
				Handles.color = Color.magenta;
			}



			MyHandles.DragHandleResult result;

			Vector3 np = net.transform.InverseTransformPoint(MyHandles.DragHandle(net.transform.TransformPoint(node.position),  actualSize, Handles.SphereCap, Color.green, out result));

			if (node.position!=np)
			{
				selectedNode = node;
				node.position = np;
				net.RecalculatePathesForNode(node);
			}



			switch (result)
			{
			case MyHandles.DragHandleResult.LMBClick:
				selectedNode = node;
				Repaint ();
				SceneView.RepaintAll();
				break;
			}


			Handles.color = Color.green;
			foreach (NetNode node2 in net.nodes)
			{


				if (node.Path(node2)<Mathf.Infinity)
				{

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

        if (selectedNode!=null)
        {
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                net.RemoveNode(selectedNode);
                net.nodes.Remove(selectedNode);
                SceneView.RepaintAll();
            }

    
            string newName = EditorGUILayout.TextField(selectedNode.nodeName, GUILayout.Width(150));
            if (newName!=selectedNode.nodeName)
            {
                selectedNode.nodeName = newName;
                SceneView.RepaintAll();
            }
        }


        EditorGUILayout.EndHorizontal();
        Handles.EndGUI();
        
	}




}
