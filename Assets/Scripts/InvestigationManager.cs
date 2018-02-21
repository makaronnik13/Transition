using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationManager : Singleton<InvestigationManager> {

    public Action<string> OnInvestigate = (string s) => { };

    private string[] defaultWrongUseItemMessages = new string[]
    {
        "Серьёзно?",
        "Дурацкий план",
        "Сомневаюсь, что получится",
        "Это невозможно!"
    };

	public void Invectigate(InteractableObject obj)
    {
        OnInvestigate.Invoke(obj.descripion);
    }

    public void Invectigate(PointAndClickItem item)
    {
        OnInvestigate.Invoke(item.description);
    }

    public void ShowDefaultWrongItemUse()
    {
        OnInvestigate.Invoke(defaultWrongUseItemMessages[UnityEngine.Random.Range(0, defaultWrongUseItemMessages.Length-1)]);
    }
}
