using GraphEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Person : MonoBehaviour
{
    public Dialog dialog;

    private DialogNode currentNode = null;
    public DialogNode CurrentNode
    {
        get
        {
            if (currentNode!=null)
            {
                currentNode = (DialogNode)dialog.nodes[0];
            }
            return currentNode;
        }
        set
        {
            currentNode = value;
        }
    }

    public void Talk()
    {
        TransmissionManager.Instance.SetTalkablePerson(this);
    }
}
