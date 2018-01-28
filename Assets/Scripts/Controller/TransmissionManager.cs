using System;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System.Linq;

public class TransmissionManager : Singleton<TransmissionManager>
{
    public Transmission startingTransmission;

    public Action<Transmission> OnTransmissionRecieved = (Transmission t) => { };
    public Action<Choice> OnChoiceApplied = (Choice t) => { };
	public Action<Transmission, Choice> OnTransmissionClosed = (Transmission t, Choice c) => { };

	Queue<Transmission> TransmissionQueue = new Queue<Transmission>();

    Transmission currentTransmission;
    Choice currentChoice;

    public void StartGame()
    {
        RunTransmission(startingTransmission);
    }

    public void ApplyChoice(int choiceIndex)
    {
		Debug.Log ("apply choice");

        currentChoice = currentTransmission.choices[choiceIndex];
        foreach (ParamEffect effect in currentChoice.paramEffects)
        {
            ParamsManager.Instance.ApplyEffect(effect);
        }
        OnChoiceApplied(currentChoice);
    }

    public void DrawTransmission ()
    {
        try{
            RunTransmission(TransmissionQueue.Dequeue());     
        }
        catch{
            Debug.Log("transmission queue is out");
            Debug.Log("StartVisualizer.Instance.ShowEnd(3);");
            //StartVisualiser.Instance.ShowEnd(3);
        }
       
    }

    public void RunTransmission (Transmission newTransmission)
    {
        if (newTransmission.isEnding)
        {
            Debug.Log("StartVisualizer.Instance.ShowEnd(newTransmission.personId);");
            StartVisualiser.Instance.ShowEnd(newTransmission.personId);
        }
        else
        {         
			currentTransmission = newTransmission;
			OnTransmissionRecieved(currentTransmission);
        }
    }

    public void CloseTransmission ()
    {
        

        foreach (Transmission transmission in currentChoice.addTransmissions)
        {
            if (!TransmissionQueue.Contains(transmission))
            {
                TransmissionQueue.Enqueue(transmission);
            }
        }

		if(TransmissionQueue.Count>0)
		{
        	TransmissionQueue = new Queue<Transmission>(TransmissionQueue.OrderBy(a=>Guid.NewGuid()));
		}

        OnTransmissionClosed(currentTransmission, currentChoice);



        if(currentChoice.nextTransmission)
        {
            RunTransmission(currentChoice.nextTransmission);    
        }
        else
        {
            Debug.Log("Next random transmission");
            DrawTransmission();   
        }
    }
}
