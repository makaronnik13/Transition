using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Model/Parameter")]
[System.Serializable]
public class GameParameter : ScriptableObject
{
    public bool displayed;
    [InlineEditor(InlineEditorModes.LargePreview)]
    public Sprite image;

    public float minValue;
    public float maxValue;
    public float defaultValue;
}
