using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class ObjectsCreator : EditorWindow
{
    public string ObjectName;
    public InteractableObject.InteractableObjectType objectType = InteractableObject.InteractableObjectType.Item;
    public Sprite sprite;
    public InteractableObject objectAsset;
    public Dialog dialog;
    public AnimatorController animatorController;

    private Texture2D DefaultSprite(InteractableObject.InteractableObjectType objectType)
    {
        switch (objectType)
        {
            case InteractableObject.InteractableObjectType.Item:
                return Resources.Load("EditorAssets/DefaultObjects/Item") as Texture2D;
            case InteractableObject.InteractableObjectType.MoveTrigger:
                return Resources.Load("EditorAssets/DefaultObjects/MoveTrigger") as Texture2D;
            case InteractableObject.InteractableObjectType.Person:
                return Resources.Load("EditorAssets/DefaultObjects/Person") as Texture2D;
        }

        return null;
    }

    [MenuItem("Window/ObjectsCreator")]
    public static void ShowWindow()
    {

        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow window = EditorWindow.GetWindow(typeof(ObjectsCreator));
        window.minSize = new Vector2(410, 50);
        window.maxSize = new Vector2(410, 1000);
        window.ShowAuxWindow();
    }

    private void OnEnable()
    {
        
    }


    void OnGUI()
    {

        ObjectName = EditorGUILayout.TextField("Name", ObjectName);
        objectType = (InteractableObject.InteractableObjectType)EditorGUILayout.EnumPopup("Type", objectType);
        sprite = (Sprite)EditorGUILayout.ObjectField("Image", sprite, typeof(Sprite), false);
        objectAsset = (InteractableObject)EditorGUILayout.ObjectField("Asset", objectAsset, typeof(InteractableObject), false);
        animatorController = (AnimatorController)EditorGUILayout.ObjectField("Animator", animatorController, typeof(AnimatorController), false);
        if (objectType == InteractableObject.InteractableObjectType.Person)
        {
            dialog = (Dialog)EditorGUILayout.ObjectField("Dialog", dialog, typeof(Dialog), false);
        }

        if (GUILayout.Button("Create"))
        {
            CreateObject();
        }


    }

    private void CreateObject()
    {
        string itemFolder = GetItemFolder(ObjectName, objectType);

        GameObject newObject = new GameObject(ObjectName);
        Texture2D texture = DefaultSprite(objectType);
        Sprite objSprite = Sprite.Create(texture, new Rect(0,0,texture.width, texture.height), Vector2.one/2);

        if (sprite)
        {
            objSprite = sprite;
        }

        newObject.AddComponent<SpriteRenderer>().sprite = objSprite;
        newObject.AddComponent<LayerController>();
        PolygonCollider2D collider = newObject.AddComponent<PolygonCollider2D>();
        PointAndClickObject pointAndClickObject = newObject.AddComponent<PointAndClickObject>();

        InteractableObject obA = objectAsset;
        if (!obA)
        {
            obA = CreateInstance<InteractableObject>();

            string assetPath = Path.Combine(itemFolder, ObjectName+".asset");
            assetPath = assetPath.Replace("\\", "/");
            assetPath = assetPath.Substring(assetPath.IndexOf("Assets"));

            AssetDatabase.CreateAsset(obA, assetPath);
        }
        pointAndClickObject.objectAsset = obA;

        AnimatorController controller = animatorController;
        if (!animatorController)
        {
            string controllerPath = Path.Combine(itemFolder, ObjectName + ".controller");
            controllerPath = controllerPath.Replace("\\", "/");
            controllerPath = controllerPath.Substring(controllerPath.IndexOf("Assets"));
            controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
        }

        Animator animator = newObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = controller;

        if (objectType == InteractableObject.InteractableObjectType.Person)
        {
            Dialog actualDialog = dialog;
            if (!actualDialog)
            {
                actualDialog = CreateInstance<Dialog>();

                string dialogPath = Path.Combine(itemFolder, ObjectName + ".asset");
                dialogPath = dialogPath.Replace("\\", "/");
                dialogPath = dialogPath.Substring(dialogPath.IndexOf("Assets"));

                AssetDatabase.CreateAsset(actualDialog, dialogPath);
            }

            newObject.AddComponent<Person>().dialog = actualDialog;
        }

        
        string prefabPath = Path.Combine(itemFolder, newObject.name + ".prefab");
        prefabPath = prefabPath.Replace("\\", "/");
        prefabPath = prefabPath.Substring(prefabPath.IndexOf("Assets"));

        UnityEngine.Object prefab = PrefabUtility.CreatePrefab(prefabPath, newObject);

        if (GameObject.Find("InteractiveObjects"))
        {
            newObject.transform.SetParent(GameObject.Find("InteractiveObjects").transform);
            newObject.transform.localScale = Vector3.one;
            newObject.transform.localRotation = Quaternion.identity;
            newObject.transform.localPosition = Vector3.zero;
            Selection.activeObject = prefab;
            Selection.activeGameObject = newObject;
        }
        else
        {
            Destroy(newObject);
        }
    }


    private string GetItemFolder(string objectName, InteractableObject.InteractableObjectType objectType)
    {
        string result = "";

        string resourcesFolder = Path.Combine(Application.dataPath, "Resources");
        string prefabsFolder = Path.Combine(resourcesFolder, "Prefabs");
        string itemsFolder = Path.Combine(prefabsFolder, "PointAndClickObjects");
        string gameItemsFolder = Path.Combine(itemsFolder, "Items");
        string triggersFolder = Path.Combine(itemsFolder, "Triggers");
        string personsFolder = Path.Combine(itemsFolder, "Persons");


        switch (objectType)
        {
            case InteractableObject.InteractableObjectType.Item:
                result = Path.Combine(gameItemsFolder, objectName);
                break;
            case InteractableObject.InteractableObjectType.MoveTrigger:
                result = Path.Combine(triggersFolder, objectName);
                break;
            case InteractableObject.InteractableObjectType.Person:
                result = Path.Combine(personsFolder, objectName);
                break;
        }

       

        if (!Directory.Exists(result))
        {
            Directory.CreateDirectory(result);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return result;
    }

    /*
    private Sprite SaveSpriteToEditorPath(Sprite baseSp, string path)
    {
        Sprite sp = Sprite.Create(baseSp.texture, new Rect(0,0,baseSp.texture.width, baseSp.texture.height), Vector3.one/2);
        SetTextureImporterFormat(baseSp.texture, true);

        string dir = Path.GetDirectoryName(path);

        Directory.CreateDirectory(dir);

        Debug.Log(path);

        File.WriteAllBytes(path, sp.texture.EncodeToPNG());
        AssetDatabase.Refresh();
        AssetDatabase.AddObjectToAsset(sp, path);
        AssetDatabase.SaveAssets();

        TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;

        ti.spritePixelsPerUnit = sp.pixelsPerUnit;
        ti.mipmapEnabled = false;
        EditorUtility.SetDirty(ti);
        ti.SaveAndReimport();

        Debug.Log(AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite);

        return AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
    }
    */

        /*
    public static void SetTextureImporterFormat(Texture2D texture, bool isReadable)
    {
        if (null == texture) return;

        string assetPath = AssetDatabase.GetAssetPath(texture);
        var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (tImporter != null)
        {
            tImporter.textureType = TextureImporterType.Default;

            tImporter.isReadable = isReadable;

            AssetDatabase.ImportAsset(assetPath);
            AssetDatabase.Refresh();
        }
    }*/
}
