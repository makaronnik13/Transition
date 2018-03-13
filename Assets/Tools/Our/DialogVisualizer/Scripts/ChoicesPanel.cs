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
			AddChoice(path, ParamsManager.Instance.CheckConditions(path.conditions));
        }
    }

	private void AddChoice(DialogStatePath path, bool enabled = true)
    {
        GameObject choiceGo = Instantiate(choicePrefab);
        choiceGo.transform.SetParent(transform);
        choiceGo.transform.localScale = Vector3.one;
		choiceGo.GetComponent<ChoiceButton>().Init(path);
		choiceGo.SetActive (enabled);
    }

	public void ApplyChoice(DialogStatePath path)
    {
		GetComponentInParent<DialogVisualizer> ().ApplyChoice (path);
                Hide();
    }

    public void Hide()
    {
        foreach (Transform tr in transform)
        {
            Destroy(tr.gameObject);
        }
    }
}
