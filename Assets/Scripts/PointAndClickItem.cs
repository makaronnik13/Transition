using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="PointAndClick/Item")]
public class PointAndClickItem : ScriptableObject {
    public string itemName;
    public string description;
    public Sprite sprite;
}
