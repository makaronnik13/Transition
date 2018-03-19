using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SaveStruct
{
    [System.Serializable]
    public class StringPair
    {
        public string S;
        public float V;

        public StringPair(string s, float v)
        {
            S = s;
            V = v;
        }
    }

	public int id;
	public string date = "";
	public string sceneName = "";
    public List<StringPair> savedParameters = new List<StringPair>();
	public string filePath = "";

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
