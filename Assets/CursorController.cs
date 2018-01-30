using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CursorController : Singleton<CursorController> {

    private List<Texture2D> cursors;
    private List<Texture2D> Cursors
    {
        get
        {
            if (cursors==null)
            {
                cursors = new List<Texture2D>();
                cursors = Resources.LoadAll<Texture2D>("Sprites/Cursors").ToList();
            }
            return cursors;
        }
    }


    public enum CursorMode
    {
        Default,
        Look,
        LookAndTalk,
        LookAndUse
    }

    private CursorMode mode = CursorMode.Default;

    private void Start()
    {
        SetMode(CursorMode.Default);
    }
    
    public void SetMode(CursorMode mode)
    {
        Cursor.SetCursor(Cursors[(int)mode], Vector2.zero, UnityEngine.CursorMode.Auto);
    }

}
