using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    public PointAndClickItem[] startingItems;

    public PointAndClickItem DraggingItem { get; internal set; }

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
}
