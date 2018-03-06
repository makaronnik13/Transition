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

    private Sprite picture;

    public void SetPicture(Texture2D pic)
    {
        picture = Sprite.Create(pic, new Rect(0,0,pic.width, pic.height), Vector2.one/2);
    }

    public Sprite GetPicture()
    {
        return picture;
    }

	public SaveStruct(string sceneName)
	{
		this.sceneName = sceneName;
	}
}
