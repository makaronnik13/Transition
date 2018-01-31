using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NarrativeNode: ScriptableObject {

#if UNITY_EDITOR
    [HideInInspector]
    public float X, Y;
    public void Drag(Vector2 p)
    {
        X = p.x;
        Y = p.y;
    }
#endif

    public Transmission transmission;

    [HideInInspector]
    public List<NarrativePath> pathes = new List<NarrativePath>();
    public string Description = "";
}
