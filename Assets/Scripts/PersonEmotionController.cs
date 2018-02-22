using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonEmotionController : MonoBehaviour {

    public PersonObject person;
	public int id;
	private bool active = false;
	public enum Emotion
	{
		Default,
        Question,
        Happy,
		Sad,
        Angry,
        Scary
	}

	public void SetEmotion(Emotion em)
	{
		GetComponentInChildren<SpriteRenderer>().sprite = person.GetEmotionSprite(em);
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


	private void Animate(Transmission t, Person p)
	{
		/*if (p.person == person) {
			GetComponent<Animator> ().SetTrigger ("Action");
			active = true;
		}*/
	}

	private void Reset(Transmission t, Choice c)
	{
		if (active) {
			GetComponentInChildren<SpriteRenderer> ().sprite = person.GetEmotionSprite(Emotion.Default);
			active = false;
		}
	}
}
