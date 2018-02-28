using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathCondition 
{
	public GameParameter parameter;
	public enum ConditionType
	{
		More,
		Less,
		Equeal,
		NotEqeal
	}
	public ConditionType conditionType;
	public int value;
}
