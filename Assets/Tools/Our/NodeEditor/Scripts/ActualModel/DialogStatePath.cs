using GraphEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogStatePath : Path {

	public string text;
	public string comentary;
	public bool automatic = false;
	public List<PathCondition> conditions = new List<PathCondition>();
	public List<ParamEffect> effects = new List<ParamEffect>();
	public bool withEvent = false;
}
