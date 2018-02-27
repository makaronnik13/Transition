using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceButton : MonoBehaviour
{
    public void Init(string text)
    {
        GetComponent<Button>().onClick.AddListener(() => { GetComponentInParent<ChoicesPanel>().ApplyChoice(this); });
        GetComponent<TextMeshProUGUI>().text = text;
    }
}
