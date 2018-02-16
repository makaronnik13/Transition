using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : Singleton<Tooltip> {

    private Text text;
    private PointAndClickObject pointingObject;

    void OnEnable()
    {
        text = GetComponentInChildren<Text>();
    }

	void Update ()
    {
        transform.position = Input.mousePosition;	
	}

    public void ShowTooltip(PointAndClickObject pointingObject)
    {
        this.pointingObject = pointingObject;
        ShowTooltip(pointingObject.objectAsset.objectName);
    }

    public void ShowTooltip(string s)
    {
        text.text = s;
    }

    public void HideTooltip()
    {
        this.pointingObject = null;
        text.text = "";
    }
}
