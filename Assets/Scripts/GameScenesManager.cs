using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;
using System.IO.Compression;

public class GameScenesManager : Singleton<GameScenesManager> {

	public string FirstScene;
	private SaveStruct currentStruct;
    public Action<SaveStruct> OnSaveLoaded = (SaveStruct) => { };

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

    private List<SaveStruct> saves = new List<SaveStruct>();

	public List<SaveStruct> Saves
	{
		get
		{
            if (saves.Count == 0)
            {
                //load saves
                string path = Path.Combine(Application.persistentDataPath, "Saves");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                foreach (string savePath in Directory.GetFiles(path))
                {
                    byte[] saveBytes = File.ReadAllBytes(savePath);

                    byte[] saveLength = new byte[4];
                    Array.Copy(saveBytes, 0, saveLength, 0, 4);
                    int saveSize = BitConverter.ToInt32(saveLength, 0);
                    byte[] save = new byte[saveSize];
                    Array.Copy(saveBytes, 4, save, 0, saveSize);

                    string json = System.Text.Encoding.UTF8.GetString(save);

                    SaveStruct ss = JsonUtility.FromJson<SaveStruct>(json);

                    byte[] picLength = new byte[4];
                    Array.Copy(saveBytes, 4 + saveSize, picLength, 0, 4);
                    int picSize = BitConverter.ToInt32(picLength, 0);
                    byte[] pic = new byte[picSize];

                    Array.Copy(saveBytes, 8 + saveSize, pic, 0, picSize);
                    Texture2D tex = new Texture2D(800, 600, TextureFormat.RGB24, false);

                    tex.LoadRawTextureData(pic);
                    tex.Apply();

                    ss.SetPicture(tex);
					ss.filePath = savePath;
                    saves.Add(ss);
                }
            }

			saves = saves.OrderByDescending (s => s.date).ToList ();

			for(int i = 0; i< saves.Count;i++)
			{
				saves [i].id = i;
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
        Save(Saves.IndexOf(currentStruct));
	}

	private void LoadScene(SaveStruct saveStruct)
	{
		if (saveStruct.date != "") {
			OnSaveLoaded.Invoke (saveStruct);
		} else 
		{
			saveStruct.sceneName = FirstScene;
		}
			currentStruct = saveStruct;
			SceneManager.LoadScene (currentStruct.sceneName);
	}

	public void LoadSafe(int i)
	{  
		if (i >= Saves.Count) 
		{
			SaveStruct ss = new SaveStruct ("");
			ss.id = i;
			LoadScene (ss);
		} else 
		{
			LoadScene (Saves [i]);
		}
	}

	public void Autosafe()
	{
		Save (currentStruct.id);
	}

	public void DestroySave(int id)
	{
		string path = Path.Combine(Application.persistentDataPath, "Saves");
		SaveStruct ss = saves.Where (s => s.id == id).ToList () [0];
		File.Delete (ss.filePath);
		saves.Remove(ss);
	}

	public void Save(int i)
	{
		SaveStruct saveStruct = CreateSaveStruct (i);
		SaveToFile (saveStruct);
		//save position
        //save scene
        //make screenshot
        //save params
        //save dictionary !!!
        //save items
	}

	private void SaveToFile(SaveStruct saveStruct)
	{
		string json = JsonUtility.ToJson(saveStruct);
		string path = Path.Combine(Application.persistentDataPath, "Saves");
		if(!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		byte[] picBytes = saveStruct.GetPicture().texture.GetRawTextureData();
		byte[] saveBytes = System.Text.Encoding.UTF8.GetBytes(json);
		byte[] saveSize = new Byte[4];
		byte[] picSize = new Byte[4];
		saveSize = System.BitConverter.GetBytes(saveBytes.Length);
		picSize = System.BitConverter.GetBytes(picBytes.Length);

		byte[] save = new byte[saveSize.Length+saveBytes.Length+picSize.Length+picBytes.Length];
		Array.Copy(saveSize, 0, save, 0, saveSize.Length);
		Array.Copy(saveBytes, 0, save, 4, saveBytes.Length);
		Array.Copy(picSize, 0, save, 4 + saveBytes.Length, picSize.Length);
		Array.Copy(picBytes, 0, save, 8 + saveBytes.Length, picBytes.Length);

		File.WriteAllBytes(Path.Combine(path,"save"+(saveStruct.id+1)+".save"), save);

		if(saveStruct.id>=Saves.Count)
		{
			Saves.Add (saveStruct);
		}
		saveStruct.filePath = Path.Combine(path,"save"+(saveStruct.id+1)+".save");
		Saves[saveStruct.id] = saveStruct;
	}

	private SaveStruct CreateSaveStruct(int id)
	{
		SaveStruct saveStruct = new SaveStruct(SceneManager.GetActiveScene().name);
		saveStruct.date = DateTime.Now.ToShortDateString()+" "+DateTime.Now.ToShortTimeString();
		saveStruct.playerPosition = FindObjectOfType<PlayerPerson>().transform.position;
		saveStruct.savedItems = Inventory.Instance.Items().Select(item=>item.itemName).ToList();
		saveStruct.savedParameters = new List<SaveStruct.StringPair>();
		saveStruct.id = id;
		foreach (KeyValuePair<string, float> pair in ParamsManager.Instance.ParamsStrings)
		{
			saveStruct.savedParameters.Add(new SaveStruct.StringPair(pair.Key, pair.Value));
		}
		Texture2D picture = ScreenshotCamera.Instance.TakePic();
		saveStruct.SetPicture(picture);

		return saveStruct;
	}

    private void Start()
    {
        saves = Saves;
    }
}
