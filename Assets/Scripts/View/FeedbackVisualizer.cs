using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackVisualizer : MonoBehaviour {

	public Text text;
	private Button button;
	private Choice currentChoice;

	// Use this for initialization
	void Start () 
	{
		button = GetComponent<Button> ();
		button.onClick.AddListener (PushTheButton);
		button.enabled = false;
		text.enabled = false;
		GetComponent<Image> ().enabled = false;
		TransmissionManager.Instance.OnChoiceApplied += ShowFeedback;
	}

	private void ShowFeedback(Choice choice)
	{
		if (choice.textFeedback != "") {
			currentChoice = choice;
			Invoke ("Show", 1.5f);
		} else 
		{
			Invoke ("Skip", 1.5f);
		}
	}

	private void Skip()
	{
		//TransmissionManager.Instance.CloseTransmission();
	}

	private void Show()
	{
		text.text = currentChoice.textFeedback;
		button.enabled = true;
		text.enabled = true;
		GetComponent<Image> ().enabled = true;
	}

	private void PushTheButton()
	{
		//TransmissionManager.Instance.CloseTransmission();
		button.enabled = false;
		text.enabled = false;
		GetComponent<Image> ().enabled = false;
	}
}
