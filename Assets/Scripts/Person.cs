using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Person : MonoBehaviour
{
    public PersonObject person;

    private NarrativeNode currentNode = null;
    public NarrativeNode CurrentNode
    {
        get
        {
            if (currentNode!=null)
            {
                currentNode = person.Dialogs.nodes[0];
            }
            return currentNode;
        }
        set
        {
            currentNode = value;
        }
    }
}
