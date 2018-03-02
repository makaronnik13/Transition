using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSound : MonoBehaviour {

	public AudioClip clip;

	// Use this for initialization
	void Start () 
	{
		SoundManager.Instance.SetMusic (clip);
	}
}
