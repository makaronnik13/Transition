using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SaveStruct 
{
	public string date;
	public string sceneName;
	public Dictionary<string, float> savedParameters = new Dictionary<string, float>();
	public List<string> savedItems = new List<string>();
	public Vector3 playerPosition;

	public SaveStruct(string sceneName)
	{
		this.sceneName = sceneName;
	}
}
