using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
public class Symbol : ScriptableObject
{
    [InlineEditor(InlineEditorModes.LargePreview)]
    public Sprite image;
}
