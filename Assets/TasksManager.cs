using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksManager : Singleton<TasksManager> {

    private PointAndClickObject activeAim;

	public void Activate(InteractableObject obj)
    {
       // FindObjectOfType<NetWalker>().
    }

    public void SetAim(PointAndClickObject pointAndClickObject)
    {
        Debug.Log("set");
        activeAim = pointAndClickObject;
    }

    public void Listen(NetWalker walker)
    {
        walker.OnStartedPath += ClearObject;
        walker.OnFinishedPath += ActivateObject;
    }

    private void ActivateObject()
    {
        if (activeAim)
        {
            activeAim.Activate();
        }
    }

    private void ClearObject()
    {
        Debug.Log("clear");
        activeAim = null;
    }

    public void StopListen(NetWalker netWalker)
    {
        netWalker.OnStartedPath -= ClearObject;
        netWalker.OnFinishedPath -= ActivateObject;
    }
}
