using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Person : MonoBehaviour
{
    public PersonDialogs dialog;

    private NarrativeNode currentNode = null;
    public NarrativeNode CurrentNode
    {
        get
        {
            if (currentNode!=null)
            {
                currentNode = dialog.nodes[0];
            }
            return currentNode;
        }
        set
        {
            currentNode = value;
        }
    }
}
