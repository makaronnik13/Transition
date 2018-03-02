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

	void Start()
	{
		GetComponent<Button> ().onClick.AddListener (ClickButton);
	}

	public void Init(string time, Sprite img)
	{
		screen.sprite = img;
		text.text = time;
	}

	public void ClickButton()
	{
		GetComponentInParent<SaveLoadPanel> ().ClickButton (transform);
	}
}
