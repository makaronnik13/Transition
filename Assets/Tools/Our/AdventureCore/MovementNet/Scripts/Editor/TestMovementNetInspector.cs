using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(MovementNet))]
public class TestMovementNetInspector : Editor
{
	//private MovementNet net;

	public override void OnInspectorGUI()
	{
		MovementNet net = (MovementNet)target;

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
}