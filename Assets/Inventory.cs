﻿using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    public PointAndClickItem[] startingItems;

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
        foreach (PointAndClickItem item in startingItems)
        {
            AddItem(item);
        }
    }

    public void AddItem(PointAndClickItem item)
    {
        GetComponentsInChildren<ItemHab>().Where(h=>h.Item == null).ToList()[0].Item = item;
    }

    public void RemoveItem(PointAndClickItem item)
    {
        ItemHab hab = GetComponentsInChildren<ItemHab>().Where(h => h.Item == item).ToList()[0];
        hab.Item = null;
    }

	public void DragItem(ItemVisual visual)
	{
		this.visual = visual;
		visual.transform.SetParent (GetComponentInParent<Canvas>().transform);
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
	}
}
