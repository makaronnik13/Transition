using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SaveStruct 
{
	public DateTime date;
	public string sceneName;
	public Dictionary<GameParameter, float> savedParameters = new Dictionary<GameParameter, float>();
	public Dictionary<PointAndClickItem, float> savedItems = new Dictionary<PointAndClickItem, float>();
	public Vector3 playerPosition;

	public SaveStruct(string sceneName)
	{
		this.sceneName = sceneName;
	}
}
