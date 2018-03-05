using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class NetNode
{
	[SerializeField]
	public string nodeName = string.Empty;
    public Vector3 position;
    private Dictionary<NetNode, float> pathes = new Dictionary<NetNode, float>();

    public float Path(NetNode node)
    {
        if (!pathes.ContainsKey(node))
        {
            pathes.Add(node, Mathf.Infinity);
        }

        return pathes[node];
    }

    public void SetPath(NetNode node, float value)
    {
        if (!pathes.ContainsKey(node))
        {
            pathes.Add(node, Mathf.Infinity);
        }
        pathes[node] = value;
    }

    public void RemovePath(NetNode removingNode)
    {
        pathes.Remove(removingNode);
    }
}
