using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO.Compression;
using System;

public class SaveButton : MonoBehaviour 
{
	public Image screen;
	public TextMeshProUGUI text;
	public Sprite PlusSprite
	{
		get
		{
			Texture2D texture = Resources.Load ("Sprites/NewGamePlus") as Texture2D;
			return Sprite.Create(texture, new Rect(0,0,texture.width, texture.height), Vector2.one/2);
		}
	}

	void Start()
	{
		GetComponent<Button> ().onClick.AddListener (ClickButton);
	}

	public void Init(string time, Sprite img, bool isNew = false)
	{
		foreach(Transform t in transform)
		{
			t.gameObject.SetActive (true);
		}
		GetComponent<Image> ().enabled = true;
		GetComponent<Button> ().enabled = true;

		screen.sprite = img;

		if (isNew) 
		{
			transform.GetChild (2).gameObject.SetActive (false);
			screen.sprite = PlusSprite;
		} else 
		{
			if(img == null)
			{
				foreach(Transform t in transform)
				{
					t.gameObject.SetActive (false);
				}
				GetComponent<Image> ().enabled = false;
				GetComponent<Button> ().enabled = false;
			}
		}
		text.text = time;
	}

	public void ClickButton()
	{
		GetComponentInParent<SaveLoadPanel> ().ClickButton (transform);
	}

	public void Delete()
	{
		GetComponentInParent<SaveLoadPanel> ().DeleteSave(transform);
	}
}
