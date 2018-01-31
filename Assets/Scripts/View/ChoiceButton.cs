using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ChoiceButton : MonoBehaviour {

	public Transform symbolsPanel;
	public Text textPanel;
    public Symbol[] specialSymbols;

	public void Init(Choice choise)
	{
		if (choise.symbolContent.Length > 0)
        {
			Destroy (textPanel.gameObject);

            foreach (Symbol symbol in choise.symbolContent)
            {
                GameObject symbolGObj = new GameObject();
                symbolGObj.transform.SetParent(symbolsPanel, false);
                symbolGObj.transform.localScale = Vector3.one;
                symbolGObj.AddComponent<Image>();
                symbolGObj.GetComponent<Image>().sprite = symbol.image;
                symbolGObj.GetComponent<Image>().color = Color.black;
                symbolGObj.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, 30f);
            }
		} else 
		{
			Destroy (symbolsPanel.gameObject);
            textPanel.enabled = true;
			textPanel.text = choise.textContent;
		}

		GetComponent<Button> ().onClick.AddListener (()=>
		{
				TransmissionManager.Instance.ApplyChoice(transform.GetSiblingIndex());
		});
	}
}
