﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : Singleton<Inventory>
{

	public Action<PointAndClickItem> OnAddItem = (PointAndClickItem i)=>{};
	public Action<PointAndClickItem> OnRemoveItem = (PointAndClickItem i)=>{};
    private List<PointAndClickItem> allItems = new List<PointAndClickItem>();
    public PointAndClickItem[] startingItems;
    private List<PointAndClickItem> items = new List<PointAndClickItem>();
    public GameObject habPrefab;

	public PointAndClickItem DraggingItem { get
		{ 
			if(visual)
			{
				return visual.Item;
			}
			return null;
		}
	}

	private ItemVisual visual;

    void Start()
    {
        GameScenesManager.Instance.OnSaveLoaded += SaveLoaded;
		SceneManager.sceneLoaded += SceneLoaded;
		SceneLoaded (SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void SaveLoaded(SaveStruct obj)
    {
        Debug.Log("scene loaded items");
        foreach (string item in obj.savedItems)
        {
            foreach (PointAndClickItem i in allItems)
            {
                if (i.itemName == item)
                {
                    AddItem(i);
                }
            }
        }

        if (obj.savedItems.Count == 0)
        {
            foreach (PointAndClickItem item in Resources.LoadAll<PointAndClickItem>("Items"))
            {
                allItems.Add(item);
            }

            foreach (PointAndClickItem item in startingItems)
            {
                AddItemToPlayer(item);
            }
        }
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (GameScenesManager.Instance.GetSceneType == GameScenesManager.SceneType.Location || GameScenesManager.Instance.GetSceneType == GameScenesManager.SceneType.MiniLocation) 
		{
			transform.GetChild (0).gameObject.SetActive (true);
            foreach(Transform t in transform.GetChild(0))
            {
                Destroy(t.gameObject);
            }
            foreach (PointAndClickItem item in items)
            {
                if (GetComponentsInChildren<ItemHab>().Where(h => h.Item == null).ToList().Count == 0)
                {
                    Instantiate(habPrefab, transform.GetChild(0), false);
                }
                GetComponentsInChildren<ItemHab>().Where(h => h.Item == null).ToList()[0].Item = item;
            }
        } else 
		{
			transform.GetChild (0).gameObject.SetActive (false);
		}
	}

	public void AddItemToPlayer(PointAndClickItem item)
	{
		OnAddItem.Invoke (item);
		AddItem (item);
	}

	public void RemoveItemFromPlayer(PointAndClickItem item)
	{
		OnRemoveItem.Invoke (item);
		RemoveItem (item);
	}


	public void AddItem(PointAndClickItem item)
    {
        items.Add(item);

        if (GameScenesManager.Instance.GetSceneType == GameScenesManager.SceneType.Location || GameScenesManager.Instance.GetSceneType == GameScenesManager.SceneType.MiniLocation)
        {
            if (GetComponentsInChildren<ItemHab>().Where(h => h.Item == null).ToList().Count == 0)
            {
                Instantiate(habPrefab, transform.GetChild(0), false);
            }
            GetComponentsInChildren<ItemHab>().Where(h => h.Item == null).ToList()[0].Item = item;
        }  
    }

	public void RemoveItem(PointAndClickItem item)
    {
        items.Remove(item);

        ItemHab hab = GetComponentsInChildren<ItemHab>().Where(h => h.Item == item).ToList()[0];
        hab.Item = null;
        Destroy(hab.gameObject);
    }

	public void DragItem(ItemVisual visual)
	{
        ItemHab hab = visual.GetComponentInParent<ItemHab>();
        this.visual = visual;
		visual.transform.SetParent (GetComponentInParent<Canvas>().transform);
        hab.Item = null;
        Destroy(hab.gameObject);
    }

    public void DropItem()
    {
        //Combine item with object
        if (Tooltip.Instance.PointingObject)
        {
            if (visual)
            {
                AddItem(visual.Item);
                Tooltip.Instance.PointingObject.UseItem(visual.Item);
                Destroy(visual.gameObject);
                visual = null;
            }
        }

        if (visual)
        {
            AddItem(visual.Item);
            Destroy(visual.gameObject);
            visual = null;
        }
			
    }

    public void DropItem(ItemHab hab)
	{
		if(visual)
		{
			hab.Item = visual.Item;
			Destroy (visual.gameObject);
			visual = null;
		}
	}

	private void Update()
	{
		if(visual)
		{
			visual.transform.position = Input.mousePosition;
		}
        if (Input.GetMouseButtonUp(0))
        {
            DropItem();
        }
	}

    public List<PointAndClickItem> Items()
    {
        List<PointAndClickItem> items = new List<PointAndClickItem>();

        foreach (ItemVisual itemVisual in GetComponentsInChildren<ItemVisual>())
        {
            items.Add(itemVisual.Item);
        }

        return items; 
    }
}