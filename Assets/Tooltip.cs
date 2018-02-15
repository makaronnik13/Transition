using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : Singleton<Tooltip> {

    private Text text;

    void OnEnable()
    {
        text = GetComponentInChildren<Text>();
    }

	void Update ()
    {
        transform.position = Input.mousePosition;	
	}

    public void ShowTooltip(string s)
    {
        text.text = s;
    }

    public void HideTooltip()
    {
        text.text = "";
    }
}
