using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceButton : MonoBehaviour
{
	
	public void Init(DialogStatePath path)
    {
		GetComponent<Button>().onClick.AddListener(() => { GetComponentInParent<ChoicesPanel>().ApplyChoice(path); });
		GetComponentInChildren<TextMeshProUGUI>().text = path.text;
    }
}
