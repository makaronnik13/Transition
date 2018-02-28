using System;
using UnityEngine.Events;

[Serializable]
public class PathEvent
{
	public DialogStatePath path;
	public UnityEvent activationEvent;

	public PathEvent(DialogStatePath path)
	{
		this.path = path;
	}
}