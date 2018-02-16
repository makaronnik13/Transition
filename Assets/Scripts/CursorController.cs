using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CursorController : Singleton<CursorController> {

	public bool changeCursor = true;

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
        LookAndUse,
        Move
    }

    private CursorMode mode = CursorMode.Default;
    public CursorMode Mode
    {
        get
        {
            return mode;
        }
        set {
			mode = value;
			if (changeCursor) {
				Cursor.SetCursor (Cursors [(int)mode], Vector2.zero, UnityEngine.CursorMode.ForceSoftware);
			}
		}
    }

    private void Start()
    {
        Mode = CursorMode.Default;
    }
    

    public void SetMode(InteractableObject.InteractableObjectType objType, bool interactable = true)
    {
        switch (objType)
        {
            case InteractableObject.InteractableObjectType.MoveTrigger:
                Mode = CursorMode.Move;
                break;
            case InteractableObject.InteractableObjectType.Person:
                Mode = CursorMode.LookAndTalk;
                break;
            case InteractableObject.InteractableObjectType.Item:
                if (interactable)
                {
                    Mode = CursorMode.LookAndUse;
                }
                else
                {
                    Mode = CursorMode.Look;
                }
                break;
        }
        
    }
}
