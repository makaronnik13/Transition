using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptableObjectUtility
{
	/// <summary>
	//	This makes it easy to create, name and place unique new ScriptableObject asset files.
	/// </summary>
	public static ScriptableObject CreateAsset<T> (string path) where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();

		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + typeof(T).ToString()+asset.GetInstanceID()+ ".asset");

		AssetDatabase.CreateAsset (asset, assetPathAndName);

		AssetDatabase.SaveAssets ();
		//AssetDatabase.Refresh();
		//EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;
		return asset;
	}
}