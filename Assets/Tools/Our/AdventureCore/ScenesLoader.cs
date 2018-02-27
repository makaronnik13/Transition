using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesLoader : Singleton<ScenesLoader>
{
    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }
}
