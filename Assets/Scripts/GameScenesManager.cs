using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScenesManager : Singleton<GameScenesManager> {

	private SaveStruct currentStruct;

	public enum SceneType
	{
		Default,
		MainMenu,
		Cinematic,
		Location,
		MiniLocation
	}

	public SceneType GetSceneType
	{
		get
		{
			if(SceneManager.GetActiveScene().name == "MainMenuScene")
			{
				return SceneType.MainMenu;
			}
			if(SceneManager.GetActiveScene().name.StartsWith("Cinematic"))
			{
				return SceneType.Cinematic;
			}
			if(SceneManager.GetActiveScene().name.StartsWith("Mini"))
			{
				return SceneType.MiniLocation;
			}
			if(SceneManager.GetActiveScene().name.StartsWith("Location"))
			{
				return SceneType.Location;
			}
			return SceneType.Default; 
		}
	}

	public List<SaveStruct> Saves
	{
		get
		{
			List<SaveStruct> saves = new List<SaveStruct> ();
			//load saves
			while(saves.Count<8)
			{
				saves.Add (new SaveStruct("CinematicScene1"));
			}

			return saves;
		}
	}

	public void ExitGame()
	{
		Application.Quit ();
	}

	public void ToMainMenu()
	{
		SaveScene ();
		SceneManager.LoadScene ("MainMenuScene");
	}

	private void SaveScene()
	{
		currentStruct.sceneName = SceneManager.GetActiveScene ().name;
	}

	private void LoadScene(SaveStruct saveStruct)
	{
		if(!GameObject.Find("AdventureCore"))
		{
			SceneManager.LoadSceneAsync ("CoreScene");
		}
		currentStruct = saveStruct;
		SceneManager.LoadScene (currentStruct.sceneName);
	}

	public void LoadSafe(int i)
	{
		LoadScene (Saves [i]);
	}

	public void Save(int i)
	{
		//save
	}
}
