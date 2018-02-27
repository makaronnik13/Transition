using GraphEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogState : NodeGraph
{
	public DialogStateNode StartNode
    {
        get
        {
            foreach (Node node in nodes)
            {
                if (((DialogStateNode)node).nodeType == DialogStateNode.StateNodeType.enter)
                {
                    return (DialogStateNode)node;
                }
            }
            return null;
        }
    }
}
