using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonEmotionController : MonoBehaviour {

	public Sprite[] emotions;
	public int id;
	private bool active = false;
	public enum Emotion
	{
		Default,
		Angry,
        Happy,
		Sad
	}

	public void SetEmotion(Emotion em)
	{
		GetComponentInChildren<SpriteRenderer>().sprite = emotions[(int)em];
	}

	void Start()
	{
		TransmissionManager.Instance.OnChoiceApplied += ShowEmotion;
		TransmissionManager.Instance.OnTransmissionRecieved += Animate;
		TransmissionManager.Instance.OnTransmissionClosed += Reset;
	}

	private void ShowEmotion(Choice c)
	{
		if (active) {
			SetEmotion (c.emotion);
			GetComponent<Animator> ().SetTrigger ("Action");
		}
		}


	private void Animate(Transmission t)
	{
		if (t.personId == id) {
			GetComponent<Animator> ().SetTrigger ("Action");
			active = true;
		}
	}

	private void Reset(Transmission t, Choice c)
	{
		if (active) {
			GetComponentInChildren<SpriteRenderer> ().sprite = emotions [0];
			active = false;
		}
	}
}
