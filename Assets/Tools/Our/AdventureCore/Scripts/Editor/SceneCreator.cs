using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCreator : EditorWindow
{
    public enum SceneType
    {
        Location,
        Puzzle,
        Cinematic
    }

    private string sceneName;
    private SceneType sceneType = SceneType.Location;
    private List<Sprite> backgrounds = new List<Sprite>();
    private AudioClip soundClip;
    private bool close = false;
    private ReorderableList list;

    [MenuItem("Window/SceneCreator")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow window = EditorWindow.GetWindow(typeof(SceneCreator));
        window.minSize = new Vector2(410, 50);
        window.maxSize = new Vector2(410, 1000);
        window.ShowAuxWindow();
    }

    private void OnEnable()
    {
        list = new ReorderableList(backgrounds, typeof(Sprite),
                true, true, true, true);
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => 
        {
            backgrounds[index] = (Sprite)EditorGUI.ObjectField(rect, backgrounds[index], typeof(Sprite), false);
        };
        list.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Backgrounds");
        };
        list.elementHeightCallback = (int index) =>
        {
            if (backgrounds[index]!=null)
            {
                return 80;
            }

            return 15;
        };
        close = false;
    }


    void OnGUI()
    {
        if (close)
        {
            Close();
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            DrawSceneSettings();
            DrawCustomSceneSettings();
            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawSceneSettings()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(300));
        sceneName = EditorGUILayout.TextField("Scene name", sceneName);
        sceneType = (SceneType)EditorGUILayout.EnumPopup("Type", sceneType);
        soundClip = (AudioClip)EditorGUILayout.ObjectField("Music", soundClip, typeof(AudioClip), false);
        if (GUILayout.Button("Generate"))
        {
            GenerateScene();
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawCustomSceneSettings()
    {
        try
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(100));
            if (sceneType == SceneType.Location)
            {
                list.DoLayoutList();
            }
            EditorGUILayout.EndVertical();
        }
        catch
        {

        }
    }
    private void GenerateScene()
    {
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        //newScene.name = sceneType.ToString() + "_" + sceneName;

        string scenesFolder = Path.Combine(Application.dataPath, "Scenes");
        if (!Directory.Exists(scenesFolder))
        {
            Directory.CreateDirectory(scenesFolder);
        }

        string LocationsFolder = Path.Combine(scenesFolder, "Locations");
        if (!Directory.Exists(LocationsFolder))
        {
            Directory.CreateDirectory(LocationsFolder);
        }
        string PuzzlesFolder = Path.Combine(scenesFolder, "Puzzles");
        if (!Directory.Exists(PuzzlesFolder))
        {
            Directory.CreateDirectory(PuzzlesFolder);
        }

        string currentPath = LocationsFolder;
        if (sceneType == SceneType.Puzzle)
        {
            currentPath = PuzzlesFolder;
        }

        currentPath =  Path.Combine(currentPath, sceneType.ToString() + "_" + sceneName+".unity");

        CreateContent();

        EditorSceneManager.SaveScene(newScene, currentPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        close = true;
        //EditorSceneManager.OpenScene(currentPath);
    }

    private void CreateContent()
    {
        GameObject movementNet = new GameObject();
        movementNet.transform.position = Vector3.zero;
        movementNet.transform.rotation = Quaternion.identity;
        movementNet.transform.localScale = Vector3.one;
        movementNet.name = "MovementNet";
        MovementNet net = movementNet.AddComponent<MovementNet>();

        for (int i =0;i<3;i++)
        {
            NetNode newNode = new NetNode();
            newNode.position = new Vector3(i*0.5f, 0, 0);
            net.nodes.Add(newNode);
            net.RecalculatePathesForNode(newNode);
        }

        GameObject objects = new GameObject();
        objects.name = "InteractiveObjects";

        GameObject background = new GameObject();
        background.name = "Background";

        for (int i = 0; i< backgrounds.Count; i++)
        {
            GameObject newBackground = new GameObject();
            newBackground.transform.SetParent(background.transform);
            newBackground.transform.localRotation = Quaternion.identity;
            newBackground.name = backgrounds[i].name;
            newBackground.transform.localPosition = Vector3.zero;
            newBackground.transform.localScale = Vector3.one;

            SpriteRenderer renderer = newBackground.AddComponent<SpriteRenderer>();
            renderer.sprite = backgrounds[i];
            renderer.sortingOrder = -100 + i;
            CamerFakeParalax paralax = newBackground.AddComponent<CamerFakeParalax>();
            paralax.force = i * 0.02f;
        }

        GameObject sceneSound = new GameObject("SceneSound");
        sceneSound.AddComponent<SceneSound>().clip = soundClip;
    }
}
