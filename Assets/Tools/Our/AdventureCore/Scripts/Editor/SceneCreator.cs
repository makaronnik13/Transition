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

        EditorSceneManager.SaveScene(newScene, currentPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        close = true;
        //EditorSceneManager.OpenScene(currentPath);
    }
}
