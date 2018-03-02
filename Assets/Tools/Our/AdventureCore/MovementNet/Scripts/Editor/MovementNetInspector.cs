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

    [CustomEditor(typeof(MovementNet))]

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

        if (GUILayout.Button("new node"))
        {
            NetNode newNode = new NetNode();
            net.nodes.Add(newNode);
            net.RecalculatePathesForNode(newNode);
            Repaint();
            SceneView.RepaintAll();
        }

		if(selectedNode!=null)
		{
			string sceneName  = EditorGUILayout.TextField ("scene",selectedNode.scene);
			if(selectedNode.scene!= sceneName)
			{
				selectedNode.scene = sceneName;
				SceneView.RepaintAll();
			}
		}
    }

    void OnSceneGUI()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.D)
        {
            if(selectedNode != null)
            {
                net.RemoveNode(selectedNode);
                net.nodes.Remove(selectedNode);
                SceneView.RepaintAll();
            }
        }
			

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.N)
        {
            Debug.Log("!");
           
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
			if (node.scene!="")
			{
				Vector2 np = Camera.current.WorldToScreenPoint (net.transform.position+node.position);

				GUILayout.BeginArea (new Rect(np, new Vector2(100, 20)));
				//EditorGUILayout.TextArea (node.scene);
				//Handles.DrawSolidRectangleWithOutline(new Rect(node.position-1*Vector3.up, new Vector2(4f, 1f)), Color.gray, Color.white);
				EditorGUILayout.LabelField( node.scene);
				GUILayout.EndArea();
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
				
			if (node.scene!="")
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
    }

    

    
}
