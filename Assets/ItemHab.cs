using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHab : MonoBehaviour {

    public GameObject itemPrefab;

    private PointAndClickItem item;
    public PointAndClickItem Item
    {
        get
        {
            return item;
        }
        set
        {
            item = value;
            if (transform.childCount==1)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            if (item)
            {
                GameObject itemGo = Instantiate(itemPrefab);
                itemGo.transform.SetParent(transform);  
                itemGo.GetComponent<ItemVisual>().Init(item);
            }
        }
    }

    void OnMouseUp()
    {
        if (Inventory.Instance.DraggingItem)
        {
            Item = Inventory.Instance.DraggingItem;
        }
    }
}
