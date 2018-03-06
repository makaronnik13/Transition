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

	public List<SaveStruct> Saves
	{
		get
		{
			List<SaveStruct> saves = new List<SaveStruct> ();
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
                int saveSize = BitConverter.ToInt32(saveLength,0);
                byte[] save = new byte[saveSize];
                Array.Copy(saveBytes, 4, save, 0, saveSize);

                string json = System.Text.Encoding.UTF8.GetString(save);

                SaveStruct ss = JsonUtility.FromJson<SaveStruct>(json);
                byte[] picLength = new byte[4];
                Array.Copy(saveBytes, 4+saveSize, picLength, 0, 4);
                int picSize = BitConverter.ToInt32(picLength, 0);
                byte[] pic = new byte[picSize];

                Array.Copy(saveBytes, 8+saveSize, pic, 0, picSize);
                Texture2D tex = new Texture2D(800, 600, TextureFormat.RGB24, false);

                tex.LoadRawTextureData(pic);
                tex.Apply();

                ss.SetPicture(tex);
                saves.Add(ss);
            }

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
   
        OnSaveLoaded.Invoke(saveStruct);
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

		string json = JsonUtility.ToJson(saveStruct);
		string path = Path.Combine(Application.persistentDataPath, "Saves");
		if(!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}


		
		byte[] picBytes = picture.GetRawTextureData();
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

        File.WriteAllBytes(Path.Combine(path,"save"+i+".save"), save);
        

		//save position
        //save scene
        //make screenshot
        //save params
        //save dictionary !!!
        //save items
	}

    
}
