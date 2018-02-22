using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogNode : ScriptableObject
{

#if UNITY_EDITOR
    [HideInInspector]
    public float X, Y;
    public void Drag(Vector2 p)
    {
        X = p.x;
        Y = p.y;
    }
#endif


    [HideInInspector]
    public List<DialogPath> pathes = new List<DialogPath>();
    public string replic = "";
}