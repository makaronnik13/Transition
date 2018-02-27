using UnityEngine;
using UnityEditor;
using System.IO;

namespace GraphEditor
{
    public static class ScriptableObjectUtility
    {
        /// <summary>
        //	This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// </summary>
        public static ScriptableObject CreateAsset<T>(string path) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();


            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "NewSo.asset");


            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();


            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(asset));

            //AssetDatabase.Refresh();
            //EditorUtility.FocusProjectWindow ();
            Selection.activeObject = asset;
            return asset;
        }

        public static ScriptableObject CreateAsset<T>(Object parent) where T : ScriptableObject
        {
            string path = AssetDatabase.GetAssetPath(parent);

            path = path.Replace(parent.name, "");
            path = path.Replace(".asset", "");

            ScriptableObject so = ScriptableObject.CreateInstance<T>();
            so.name = "New " + typeof(T);
            AssetDatabase.AddObjectToAsset(so, parent);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(parent));

            return so;
        }
    }
}