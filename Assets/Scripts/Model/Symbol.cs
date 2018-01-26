using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Model/Symbol")]
[System.Serializable]
public class Symbol : ScriptableObject
{
    [InlineEditor(InlineEditorModes.LargePreview)]
    public Sprite image;
}
