using System;
using UnityEngine.Events;

[Serializable]
public class ItemEvent
{
    public PointAndClickItem item;
    public UnityEvent activationEvent;

    public ItemEvent(PointAndClickItem item)
    {
        this.item = item;
    }
}