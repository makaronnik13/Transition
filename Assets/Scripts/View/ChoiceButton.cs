using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ChoiceButton : MonoBehaviour {

	public Transform symbolsPanel;
	public Text textPanel;
    public Symbol[] specialSymbols;
    public GameObject symbolsRaw;

	public void Init(Choice choise)
	{
		if (choise.symbolContent.Length > 0)
        {
			Destroy (textPanel.gameObject);

            Transform currentRaw = null;
            foreach (Symbol symbol in choise.symbolContent)
            {
                if (specialSymbols.ToList().Contains(symbol))
                {
                    currentRaw = Instantiate(symbolsRaw).transform;
                    currentRaw.transform.SetParent(symbolsPanel, false);
                    // Set height to 30
                    currentRaw.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
                    currentRaw.transform.localScale = Vector3.one;
                }

                GameObject symbolGObj = new GameObject();
                symbolGObj.transform.SetParent(currentRaw, false);
                symbolGObj.transform.localScale = Vector3.one;
                symbolGObj.AddComponent<Image>();
                symbolGObj.GetComponent<Image>().sprite = symbol.image;
                symbolGObj.GetComponent<Image>().color = Color.black;
                symbolGObj.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, 30f);
            }
		} else 
		{
			Destroy (symbolsPanel.gameObject);
			textPanel.text = choise.textContent;
		}

		GetComponent<Button> ().onClick.AddListener (()=>
		{
				TransmissionManager.Instance.ApplyChoice(transform.GetSiblingIndex());
		});
	}
}
