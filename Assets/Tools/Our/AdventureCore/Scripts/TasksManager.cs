
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksManager : Singleton<TasksManager> {

    private PointAndClickObject activeAim;
    private PointAndClickItem activeItem;

	public void Activate(InteractableObject obj)
    {
       // FindObjectOfType<NetWalker>().
    }

    public void SetAim(PointAndClickObject pointAndClickObject)
    {
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
            if (activeItem)
            {
                activeAim.ApplyItem(activeItem);
                activeItem = null;
            }
            else
            {

                activeAim.Activate();
            }
        }
    }
    

    private void ClearObject()
    {
        activeAim = null;
        activeItem = null;
    }

    public void StopListen(NetWalker netWalker)
    {
        netWalker.OnStartedPath -= ClearObject;
        netWalker.OnFinishedPath -= ActivateObject;
    }

    public void SetItem(PointAndClickItem item)
    {
        activeItem = item;
    }
}
