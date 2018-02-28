using System;
using UnityEngine.Events;

[Serializable]
public class StateEvent
{
	public DialogStateNode node;
	public UnityEvent activationEvent;

	public StateEvent(DialogStateNode node)
	{
		this.node = node;
	}
}
