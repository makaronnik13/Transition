using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackVisualizer : MonoBehaviour {

	public Text text;
	private Button button;

	// Use this for initialization
	void Start () 
	{
		button = GetComponent<Button> ();
		button.onClick.AddListener (()=>{
			TransmissionManager.Instance.CloseTransmission();
			button.enabled = false;
			text.enabled = false;
			GetComponent<Image> ().enabled = false;
		});
		button.enabled = false;
		text.enabled = false;
		GetComponent<Image> ().enabled = false;
		TransmissionManager.Instance.OnChoiceApplied += ShowFeedback;
	}

	private void ShowFeedback(Choice choice)
	{
		if(choice.textContent!="")
		{
			text.text = choice.textFeedback;
			button.enabled = true;
			text.enabled = true;
			GetComponent<Image> ().enabled = true;
		}
	}
}
