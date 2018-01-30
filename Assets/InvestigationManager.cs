using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationManager : Singleton<InvestigationManager> {

    public Action<string> OnInvestigate = (string s) => { };

	public void Invectigate(InvestigatedItem item)
    {
        OnInvestigate.Invoke(item.Description);
    }
}
