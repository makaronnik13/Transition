using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadPanel : MonoBehaviour 
{
	public enum SaveLoadPanelType
	{
		Save,
		Load
	}

	public void SetPanelType(int i)
	{
		panelType = (SaveLoadPanelType)i;
	}

	public SaveLoadPanelType panelType = SaveLoadPanelType.Load;

	void OnEnable()
	{
		int i = 0;
		bool createPlus = true;

		foreach(SaveButton panel in GetComponentsInChildren<SaveButton>())
		{
			if (GameScenesManager.Instance.Saves.Count > i) 
			{
				SaveStruct ss = GameScenesManager.Instance.Saves [i];
				panel.Init (GameScenesManager.Instance.Saves [i].date, ss.GetPicture ());	
			} else 
			{

					panel.Init("", null, createPlus);
					createPlus = false;
			}
			i++;
		}
	}


	public void ClickButton (Transform buttonTransform)
	{
		int i = 0;
		foreach (SaveButton panel in GetComponentsInChildren<SaveButton>()) 
		{
			if(panel.transform == buttonTransform)
			{
				if (panelType == SaveLoadPanelType.Load) {
					TryToLoadScene (i);
				} else 
				{
					GameScenesManager.Instance.Save (i);
                    GetComponentInParent<MainMenuController>().HideMenu();

                }
			}
			i++;
		}
	}

	public void DeleteSave (Transform buttonTransform)
	{
		int i = 0;
		foreach (SaveButton panel in GetComponentsInChildren<SaveButton>()) 
		{
			if(panel.transform == buttonTransform)
			{
				GameScenesManager.Instance.DestroySave (i);
			}
			i++;
		}

		OnEnable ();
	}

	private void TryToLoadScene(int i)
	{
		GameScenesManager.Instance.LoadSafe (i);
	}
}
