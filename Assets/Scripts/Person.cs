using GraphEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Person : MonoBehaviour
{
    public Dialog dialog;

	public List<PathEvent> pathEvents = new List<PathEvent>();
	public List<StateEvent> nodeEvents = new List<StateEvent>();

	void Start()
	{
		TransmissionManager.Instance.OnPathGo += PathGo;
		TransmissionManager.Instance.OnNodeIn += NodeIn;
	}

	private void PathGo(DialogStatePath path)
	{
		if(path.withEvent)
		{
			PathEvent pathEvent = pathEvents.Find (p=>p.path == path);
			if(pathEvent!=null)
			{
				pathEvent.activationEvent.Invoke ();
			}
		}
	}

	private void NodeIn(DialogStateNode node)
	{
		if(node.withEvent)
		{
			StateEvent stateEvent = nodeEvents.Find (p=>p.node == node);
			if(stateEvent!=null)
			{
				stateEvent.activationEvent.Invoke ();
			}
		}
	}

    private DialogNode currentNode;
    public DialogNode CurrentNode
    {
        get
        {
            if (currentNode==null)
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

    public void TalkAbout(string nodeName)
    {
        DialogNode node = (DialogNode)dialog.nodes.Find(n => n.name == nodeName);
        if (node)
        {
            TransmissionManager.Instance.TalkAbout(this, node);
        }
    }

	public void Test(string s)
	{
		Debug.Log (s);
	}
}
