
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : Singleton<Tooltip> {

    private TextMeshProUGUI text;
    public PointAndClickObject PointingObject;

    void OnEnable()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

	void Update ()
    {
        transform.position = Input.mousePosition+Vector3.up*20;	
	}

    public void ShowTooltip(PointAndClickObject pointingObject)
    {
        this.PointingObject = pointingObject;
        string text = "";
        if (Inventory.Instance.DraggingItem)
        {
            text += Inventory.Instance.DraggingItem.itemName + "+";
        }
        text += pointingObject.objectAsset.objectName;
        ShowTooltip(text);
    }

    public void ShowTooltip(string s)
    {
        text.text = s;
    }

    public void HideTooltip()
    {
        this.PointingObject = null;
        text.text = "";
    }
}
