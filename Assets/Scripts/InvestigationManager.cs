using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationManager : Singleton<InvestigationManager> {

    public Action<string> OnInvestigate = (string s) => { };

	public void Invectigate(InteractableObject obj)
    {
        OnInvestigate.Invoke(obj.descripion);
    }

    public void Invectigate(PointAndClickItem item)
    {
        OnInvestigate.Invoke(item.description);
    }
}
