using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChoicesPanel : MonoBehaviour {

    public GameObject choicePrefab;

    public void ShowVariants(DialogStateNode node)
    {
        foreach (Transform tr in transform)
        {
            Destroy(tr.gameObject);
        }

        foreach (DialogStatePath path in node.pathes)
        {
            AddChoice(path.text);
        }
    }

    private void AddChoice(string text)
    {
        GameObject choiceGo = Instantiate(choicePrefab);
        choiceGo.transform.SetParent(transform);
        choiceGo.transform.localScale = Vector3.one;
        choiceGo.GetComponent<ChoiceButton>().Init(text);
    }

    public void ApplyChoice(ChoiceButton choiceButton)
    {
        int i = 0;
        foreach (ChoiceButton cb in GetComponentsInChildren<ChoiceButton>())
        {
            if (cb == choiceButton)
            {
                TransmissionManager.Instance.SelectDialogVariant(i);
                Hide();
            }
            i++;
        }
    }

    public void Hide()
    {
        foreach (Transform tr in transform)
        {
            Destroy(tr.gameObject);
        }
    }
}
