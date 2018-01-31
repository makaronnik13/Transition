using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Model/Dialog")]

[System.Serializable]
public class PersonDialogs : ScriptableObject {

    public List<NarrativeNode> nodes = new List<NarrativeNode>();

}
