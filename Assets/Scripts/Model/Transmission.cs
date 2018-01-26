using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
[CreateAssetMenu(menuName = "Model/Transmission")]
public class Transmission : ScriptableObject
{
    public Symbol[] content;
    public Choice[] choices;
}
