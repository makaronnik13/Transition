using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalEntery : MonoBehaviour {

	public Transform symbols, answer;
	public Text answerText;
	public Text feedbackText;


	public void Init(Transmission transmission, Choice choise)
	{
		feedbackText.text = choise.textFeedback;
		if (choise.symbolContent.Length == 0) {
			Destroy (answer.gameObject);
			answerText.text = choise.textContent;
		} else {
			Destroy (answerText.gameObject);
			foreach(Symbol symbolValue in choise.symbolContent)
			{
				GameObject symbol = new GameObject ();
				symbol.transform.SetParent (answer);
				symbol.transform.localScale = Vector3.one;
				symbol.AddComponent<Image> ().sprite = symbolValue.image;
				symbol.GetComponent<Image> ().color = Color.black;
			}
		}

		foreach(Symbol symbolValue in transmission.content)
		{
			GameObject symbol = new GameObject ();
			symbol.transform.SetParent (symbols);
			symbol.transform.localScale = Vector3.one;
			symbol.AddComponent<Image> ().sprite = symbolValue.image;
			symbol.GetComponent<Image> ().color = Color.black;
		}
	}
}
