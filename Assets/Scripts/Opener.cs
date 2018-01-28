using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opener : MonoBehaviour {

	public void Open(){
		GetComponent<Animator> ().SetBool ("Open", true);
	}

	public void Close(){
		GetComponent<Animator> ().SetBool ("Open", false);
	}
}
