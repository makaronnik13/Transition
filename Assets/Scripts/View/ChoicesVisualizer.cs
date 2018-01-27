using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoicesVisualizer : MonoBehaviour {

	public GameObject choicePrefab;

	public void Start()
	{
		TransmissionManager.Instance.OnTransmissionRecieved += DrawTransmission;
		TransmissionManager.Instance.OnChoiceApplied += Hide;
	}

	private void DrawTransmission(Transmission transmission)
	{
		foreach(Choice c in transmission.choices)
		{
			GameObject newChoice = Instantiate (choicePrefab);
			newChoice.transform.SetParent (transform);
			newChoice.transform.localScale = Vector3.one;
			newChoice.GetComponent<ChoiceButton> ().Init (c);
		}
	}

	private void Hide(Choice c)
	{
		foreach(Transform t in transform)
		{
			Destroy (t.gameObject);
		}
	}
}
