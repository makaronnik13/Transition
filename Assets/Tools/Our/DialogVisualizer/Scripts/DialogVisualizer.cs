
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogVisualizer : Singleton<DialogVisualizer>
{
    private DialogStateNode node;

    private PersonText PersonTextVisualizer
    {
        get
        {
            return GetComponentInChildren<PersonText>();
        }
    }

    private ChoicesPanel ChoiceVariantsVisualizer
    {
        get
        {
            return GetComponentInChildren<ChoicesPanel>();
        }
    }

    public void Start()
    {
        TransmissionManager.Instance.OnNodeIn += ShowNode;
        TransmissionManager.Instance.OnPersonChanged += PersonChanged;
        TransmissionManager.Instance.OnDialogFinished += HideDialog;
        PersonTextVisualizer.OnPhraseFinished += PhraseFinished;
    }

    private void HideDialog()
    {
			
		ChoiceVariantsVisualizer.Hide ();
		PersonTextVisualizer.Hide ();
    }
		
    private void PhraseFinished()
    {
        ChoiceVariantsVisualizer.ShowVariants(node);
    }

    private void PersonChanged(Person obj)
    {
        PersonTextVisualizer.person = obj.transform;
    }

    private void ShowNode(DialogStateNode obj)
    {
		
		if(GameScenesManager.Instance.GetSceneType == GameScenesManager.SceneType.Location)
		{
        	node = obj;
        
        	PersonTextVisualizer.ShowFeedback(obj.text);
		}
    }

	public void ApplyChoice(DialogStatePath path)
	{
		TransmissionManager.Instance.SelectDialogVariant(path);
	}
}
