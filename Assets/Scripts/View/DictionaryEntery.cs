using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryEntery : MonoBehaviour {

	public Image symbolIcon;

	public void Init(Symbol symbol)
	{
		symbolIcon.sprite = symbol.image;
	}
}
