using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphEditor;

[CreateAssetMenu(menuName = "Model/ParameterCollection")]
public class ParamCollection : ScriptableObject 
{

	public List<GameParameter> Parameters = new List<GameParameter>();
}
