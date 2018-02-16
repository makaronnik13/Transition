using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointAndClickObject : MonoBehaviour
{
    public InteractableObject objectAsset;

    public UnityEvent onActivation;

    private bool focused = false;

    void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject() || focused)
        {
            // we're over a UI element... peace out
            return;
        }

        Tooltip.Instance.ShowTooltip(this);
        focused = true;
        CursorController.Instance.SetMode(objectAsset.objType, objectAsset.interactable);
    }

    void OnMouseExit()
    {
        Tooltip.Instance.HideTooltip();
        focused = false;
        CursorController.Instance.Mode = CursorController.CursorMode.Default;
    }

    private void Update()
    {

        if (!focused)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TasksManager.Instance.SetAim(this);
            OnMouseExit();
        }

        if (Input.GetMouseButtonDown(1))
        {
            InvestigationManager.Instance.Invectigate(objectAsset);
            OnMouseExit();
        }
    }

    public void Activate()
    {
        onActivation.Invoke();
        if (objectAsset.objType == InteractableObject.InteractableObjectType.MoveTrigger)
        {
            ScenesLoader.Instance.LoadScene(objectAsset.sceneId);
        }

        if (objectAsset.objType == InteractableObject.InteractableObjectType.Person)
        {
            TransmissionManager.Instance.SetTalkablePerson(GetComponent<Person>());
        }
    }

    public void UseItem(PointAndClickItem item)
    {
    
    }
}
