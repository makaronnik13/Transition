using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;
using System.IO.Compression;

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
        SaveStruct saveStruct = new SaveStruct(SceneManager.GetActiveScene().name);
		saveStruct.date = DateTime.Now.ToShortDateString()+" "+DateTime.Now.ToShortTimeString();
        saveStruct.playerPosition = FindObjectOfType<PlayerPerson>().transform.position;
		saveStruct.savedItems = Inventory.Instance.Items().Select(item=>item.itemName).Distinct().ToList();
		saveStruct.savedParameters = ParamsManager.Instance.ParamsStrings;
        
        Texture2D picture = ScreenshotCamera.Instance.TakePic();

		Debug.Log (Application.persistentDataPath);

		string json = JsonUtility.ToJson(saveStruct);
		string path = Path.Combine(Application.dataPath, "Saves");
		if(!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		string tempPath = Path.Combine(path, "Temp");
		if(!Directory.Exists(tempPath))
		{
			Directory.CreateDirectory(tempPath);
		}
		StreamWriter writer = new StreamWriter(Path.Combine(tempPath, "Save"));
		writer.Write(json);
		writer.Close();
		byte[] picBytes = picture.EncodeToPNG();
		File.WriteAllBytes(Path.Combine(tempPath,"screen.png"), picBytes);

	    
		//save position
        //save scene
        //make screenshot
        //save params
        //save dictionary !!!
        //save items
	}
}
