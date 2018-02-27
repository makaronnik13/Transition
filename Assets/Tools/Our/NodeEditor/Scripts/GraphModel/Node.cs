using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphEditor
{
    [System.Serializable]
    public class Node : GUIDraggableObject
    {
        public override void Init(Vector2 position)
        {
            base.Init(position);
            pathes = new List<Path>();
        }

        public List<Path> pathes = new List<Path>();
    }
}
