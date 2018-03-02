using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleSceneLoader : MonoBehaviour {

	public string sceneName;
	public bool loadAtStart = false;

	void Start()
	{
		if(loadAtStart)
		{
			Load ();
		}
	}

	public void Load()
	{
		SceneManager.LoadScene (sceneName);
	}
}
