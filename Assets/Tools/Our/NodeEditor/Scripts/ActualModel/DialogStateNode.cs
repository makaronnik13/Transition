using GraphEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogStateNode : Node 
{
	public string text;

	public enum StateNodeType
	{
		simple,
		enter,
		exit,
		narrativeExit
	}

	public StateNodeType nodeType = StateNodeType.simple;

	public string comentary;

	public DialogPath exitPath;

	public List<ParamEffect> effects = new List<ParamEffect>();

	public bool withEvent = false;
}
