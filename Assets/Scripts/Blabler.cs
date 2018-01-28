using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blabler : MonoBehaviour {

	public AudioClip[] clips1, clips2, clips3;

	// Use this for initialization
	void Start () {
		TransmissionManager.Instance.OnTransmissionRecieved += RandomBla;
	}
	
	private void RandomBla(Transmission tr)
	{
		switch (tr.personId) 
		{
		case 1:
			GetComponent<AudioSource> ().PlayOneShot (clips1[Random.Range(0,clips1.Length-1)]);
			break;
		case 0:
			GetComponent<AudioSource> ().PlayOneShot (clips2[Random.Range(0,clips2.Length-1)]);
			break;
		case 2:
			GetComponent<AudioSource> ().PlayOneShot (clips3[Random.Range(0,clips3.Length-1)]);
			break;
		}

	}
}
