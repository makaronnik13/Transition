﻿using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SymbolsVisualizer : MonoBehaviour
{
	private Image background;
	public Transform[] pivots;
	public GameObject symbolsRaw;

	public Symbol[] specialSymbols;

    private void Start()
    {
		background = transform.GetChild (0).GetComponent<Image> ();
        TransmissionManager.Instance.OnTransmissionRecieved += DrawTransmission;
		TransmissionManager.Instance.OnChoiceApplied += Hide;
    }

    private void DrawTransmission (Transmission transmission)
    {
		Transform currentRaw = null;

        foreach (Symbol symbol in transmission.content)
        {
			if(specialSymbols.ToList().Contains(symbol))
			{
				currentRaw = Instantiate (symbolsRaw).transform;
				currentRaw.transform.SetParent (transform, false);
				currentRaw.transform.localScale = Vector3.one;
			}

            GameObject symbolGObj = new GameObject();
			symbolGObj.transform.SetParent(currentRaw, false);
			symbolGObj.transform.localScale = Vector3.one;
			symbolGObj.AddComponent<Image>();
			symbolGObj.GetComponent<Image>().sprite = symbol.image;
			symbolGObj.GetComponent<Image> ().color = Color.black;
			symbolGObj.GetComponent<RectTransform> ().sizeDelta = new Vector2 (55f, 55f);
        }

		transform.SetParent(pivots[transmission.personId]);
		transform.localPosition = Vector3.zero;
		background.enabled = true;
    }

    private void Hide (Choice choice)
    {
        foreach (Transform symbolChild in transform)
        {
			if(symbolChild!=background.transform)
			{
				Destroy(symbolChild.gameObject);	
			}
        }
		background.enabled = false;
    }
}
