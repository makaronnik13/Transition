using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	void Start()
	{
		SceneManager.sceneLoaded += SceneLoaded;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape) && GameScenesManager.Instance.GetSceneType != GameScenesManager.SceneType.MainMenu && GameScenesManager.Instance.GetSceneType != GameScenesManager.SceneType.Cinematic && GameScenesManager.Instance.GetSceneType != GameScenesManager.SceneType.MiniLocation)
		{
			if(transform.GetChild(3).gameObject.activeInHierarchy)
			{
				HideMenu ();	
			}
			else{
			SetMenu (3);
			//pause game
			}
		}
	}

	private void SceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (GameScenesManager.Instance.GetSceneType == GameScenesManager.SceneType.MainMenu) 
		{
			SetMenu (0);
		} else 
		{
			HideMenu ();
		}
	}

	public void Exit()
	{
		Application.Quit ();
	}

	public void SetMenu(int i)
	{
		if(i == 0 && GameScenesManager.Instance.GetSceneType != GameScenesManager.SceneType.MainMenu)
		{
			i = 3;
		}

		foreach(Transform t in transform)
		{
			t.gameObject.SetActive (false);
		}
		transform.GetChild (i).gameObject.SetActive (true);
	}

	public void HideMenu()
	{
		foreach(Transform t in transform)
		{
			t.gameObject.SetActive (false);
		}
	}

	public void ReturnToMainMenu()
	{
		Debug.LogWarning ("save");
		SceneManager.LoadScene ("MainMenuScene");
	}
}
