using UnityEngine;

[System.Serializable]
public class GameParameter : ScriptableObject
{
    public Sprite image;
    public float minValue;
    public float maxValue = 100;
    public float defaultValue;
}
