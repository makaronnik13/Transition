using System;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System.Linq;

public class TransmissionManager : Singleton<TransmissionManager>
{
    public Transmission startingTransmission;

    Action<Transmission> OnTransmissionRecieved = (Transmission t) => { };
    Action<Choice> OnChoiceApplied = (Choice t) => { };
    Action<Transmission, Choice> OnTransmissionClosed = (Transmission t, Choice c) => { };

    Queue<Transmission> TransmissionQueue;

    Transmission currentTransmission;
    Choice currentChoice;

    private void Start()
    {
        RunTransmission(startingTransmission);
    }

    public void ApplyChoice(int choiceIndex)
    {
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
        }
       
    }

    public void RunTransmission (Transmission newTransmission)
    {
        currentTransmission = newTransmission;
        OnTransmissionRecieved(currentTransmission);
    }

    public void CloseTransmission ()
    {
        

        foreach (Transmission transmission in currentChoice.addTransmissions)
        {
            TransmissionQueue.Enqueue(transmission);
        }

        TransmissionQueue = new Queue<Transmission>(TransmissionQueue.OrderBy(a=>Guid.NewGuid()));

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
