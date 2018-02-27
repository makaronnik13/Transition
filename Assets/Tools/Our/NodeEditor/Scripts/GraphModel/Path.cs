using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphEditor
{
    [System.Serializable]
    public class Path : ScriptableObject
    {
        private Node start;
        public Node Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }

        private Node end;
        public Node End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
            }
        }

        public void Init(Node startNode, Node endNode)
        {
            start = startNode;
            start.pathes.Add(this);
            end = endNode;
        }
    }
}
