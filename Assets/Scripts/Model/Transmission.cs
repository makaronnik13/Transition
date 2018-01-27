using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
[CreateAssetMenu(menuName = "Model/Transmission")]
public class Transmission : ScriptableObject
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
	public Symbol[] content = new Symbol[0];
	public Choice[] choices = new Choice[0];
}
