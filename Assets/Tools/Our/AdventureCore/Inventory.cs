using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    public PointAndClickItem[] startingItems;

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
        foreach (PointAndClickItem item in startingItems)
        {
            AddItem(item);
        }
    }

    public void AddItem(PointAndClickItem item)
    {
        if (GetComponentsInChildren<ItemHab>().Where(h => h.Item == null).ToList().Count == 0)
        {
            Instantiate(habPrefab, transform, false);
        }

        GetComponentsInChildren<ItemHab>().Where(h=>h.Item == null).ToList()[0].Item = item;
    }

    public void RemoveItem(PointAndClickItem item)
    {
        ItemHab hab = GetComponentsInChildren<ItemHab>().Where(h => h.Item == item).ToList()[0];
        hab.Item = null;
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
}
