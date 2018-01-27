using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalVisualizer : MonoBehaviour {

	public Transform content;
	public GameObject enteryPrefab;

	// Use this for initialization
	void Start () {
		TransmissionManager.Instance.OnTransmissionClosed += AddEntery;	
	}

	private void AddEntery(Transmission transmission, Choice choice)
	{
		GameObject newEntery = Instantiate (enteryPrefab);
		newEntery.transform.SetParent (content);
		newEntery.transform.localScale = Vector3.one;
		newEntery.GetComponent<JournalEntery> ().Init (transmission, choice);
	}
}
