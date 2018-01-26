using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour {

	public Transform symbolsPanel;
	public Text textPanel;

	public void Init(Choice choise)
	{
		if (choise.symbolContent.Length > 0) {
			Destroy (textPanel.gameObject);
			foreach(Symbol s in choise.symbolContent)
			{
				GameObject symbol = new GameObject ();
				symbol.transform.SetParent (symbolsPanel);
				symbol.AddComponent<Image> ().sprite = s.image;
			}
		} else 
		{
			Destroy (symbolsPanel.gameObject);
			textPanel.text = choise.textContent;
		}

		GetComponent<Button> ().onClick.AddListener (()=>
		{
				//TransitionsManager.Instance.	
		});
	}
}
