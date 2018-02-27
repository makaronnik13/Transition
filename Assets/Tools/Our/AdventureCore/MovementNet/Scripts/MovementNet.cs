using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MovementNet : MonoBehaviour {
  
    public float minDistance = 1;
    public List<NetNode> nodes = new List<NetNode>();
    public float minSize = 0.3f;
    public float maxSize = 2f;

    private void Awake()
    {
        foreach (NetNode nn in nodes)
        {
            RecalculatePathesForNode(nn);
        }
    }


    public NetNode GetNearestPoint(Vector3 position)
    {
        Vector3 point = transform.InverseTransformPoint(position);
        return nodes.OrderBy(n=>Vector3.Distance(n.position, point)).ToList()[0];
    }



    public List<Vector3> ShortestPath(Vector3 from, Vector3 aim)
    {
        List<Vector3> result = new List<Vector3>();
        List<NetNode> nnList = ShortestPath(GetNearestPoint(from), GetNearestPoint(aim), nodes.ToArray());
        if (nnList.Count>1)
        {
            if (Vector3.Distance(GetNodeWorldPosition(nnList[0]), aim)> Vector3.Distance(GetNodeWorldPosition(nnList[1]), aim))
            {
                nnList.RemoveAt(0);
            }
        }

        foreach (NetNode nn in nnList)
        {
            

            result.Add(GetNodeWorldPosition(nn));
        }
        return result;
    }

    public Vector3 GetNodeWorldPosition(NetNode currentAimPoint)
    {
        return transform.TransformPoint(currentAimPoint.position);
    }

    public void RecalculatePathesForNode(NetNode node)
    {
        foreach (NetNode node2 in nodes)
        {
            if (node == node2)
            {
                continue;
            }


            float dist = Vector3.Distance(node.position, node2.position);


            if (dist >= minDistance)
            {
                dist = Mathf.Infinity;
            }


            node.SetPath(node2, dist);
            node2.SetPath(node, dist);
        }
    }

    public void RemoveNode(NetNode removingNode)
    {
        foreach (NetNode node in nodes)
        {
            node.RemovePath(removingNode);
        }
    }

    public List<NetNode> ShortestPath(NetNode start, NetNode end, NetNode[] allNodes)
    {
        // We don't accept null arguments
        if (start == null || end == null)
        {
            throw new ArgumentNullException();
        }

        // The final path
        List<NetNode> path = new List<NetNode>();

        // If the start and end are same node, we can return the start node
        if (start == end)
        {
            path.Add(start);
            return path;
        }

        // The list of unvisited nodes
        List<NetNode> unvisited = new List<NetNode>();

        // Previous nodes in optimal path from source
        Dictionary<NetNode, NetNode> previous = new Dictionary<NetNode, NetNode>();

        // The calculated distances, set all to Infinity at start, except the start Node
        Dictionary<NetNode, float> distances = new Dictionary<NetNode, float>();

        for (int i = 0; i < allNodes.Length; i++)
        {
            NetNode node = allNodes[i];
            unvisited.Add(node);

            // Setting the node distance to Infinity
            distances.Add(node, float.MaxValue);
        }

        // Set the starting Node distance to zero
        distances[start] = 0f;
        while (unvisited.Count != 0)
        {

            // Ordering the unvisited list by distance, smallest distance at start and largest at end
            unvisited = unvisited.OrderBy(node => distances[node]).ToList();

            // Getting the Node with smallest distance
            NetNode current = unvisited[0];

            // Remove the current node from unvisisted list
            unvisited.Remove(current);

            // When the current node is equal to the end node, then we can break and return the path
            if (current == end)
            {

                // Construct the shortest path
                while (previous.ContainsKey(current))
                {

                    // Insert the node onto the final result
                    path.Insert(0, current);

                    // Traverse from start to end
                    current = previous[current];
                }

                // Insert the source onto the final result
                path.Insert(0, current);
                break;
            }

            // Looping through the Node connections (neighbors) and where the connection (neighbor) is available at unvisited list
            for (int i = 0; i < allNodes.Length; i++)
            {
                if (current.Path(allNodes[i]) == Mathf.Infinity)
                {
                    continue;
                }

                NetNode neighbor = allNodes[i];

                // Getting the distance between the current node and the connection (neighbor)
                float length = current.Path(neighbor);

                // The distance from start node to this connection (neighbor) of current node
                float alt = distances[current] + length;

                // A shorter path to the connection (neighbor) has been found
                if (alt < distances[neighbor])
                {
                    distances[neighbor] = alt;
                    previous[neighbor] = current;
                }
            }
        }
        return path;
    }

    public Vector3 GetScale(Vector3 aim, Vector3 from, Vector3 pos)
    {

        float coef = Vector3.Distance(aim, pos)/Vector3.Distance(aim, from);

        if (aim == from)
        {
            coef = 1;
        }

        NetNode fromPoint = GetNearestPoint(from);
        NetNode aimPoint = GetNearestPoint(aim);

        List<NetNode> orderedNodes = nodes.OrderBy(n=>n.position.y).ToList();
        float d = orderedNodes[orderedNodes.Count() - 1].position.y - orderedNodes[0].position.y;
        float scale1 = (orderedNodes[orderedNodes.Count() - 1].position.y - fromPoint.position.y) / d;
        float scale2 = (orderedNodes[orderedNodes.Count() - 1].position.y - aimPoint.position.y) / d;
        return Vector3.one * Mathf.Clamp(Mathf.Lerp(scale1, scale2, 1-coef), minSize, maxSize);
            
    }
}


