using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="PointAndClick/Object")]
public class InteractableObject : ScriptableObject {

    public enum InteractableObjectType
    {
        Item,
        Person,
        MoveTrigger
    }

    public InteractableObjectType objType;

    public bool interactable = true;

    public string objectName;
    public string descripion;

    public Combinations[] combinations;
    public string sceneName;
}
