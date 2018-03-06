using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadPanel : MonoBehaviour {
	
	public Sprite emptySprite;

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
		foreach(SaveButton panel in GetComponentsInChildren<SaveButton>())
		{
            if (GameScenesManager.Instance.Saves.Count>i)
            {
                panel.Init(GameScenesManager.Instance.Saves[i].date, GameScenesManager.Instance.Saves[i].GetPicture());
            }
            else
            {
                panel.Init("", emptySprite);
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
				}
			}
			i++;
		}
	}

	private void TryToLoadScene(int i)
	{
		GameScenesManager.Instance.LoadSafe (i);
	}
}
