using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer), typeof(PolygonCollider2D))]
public class PointAndClickObject : MonoBehaviour
{
    public InteractableObject objectAsset;

    public UnityEvent onActivation;

    public List<ItemEvent> itemsEvents = new List<ItemEvent>();

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

    public void ApplyItem(PointAndClickItem item)
    {
        if (objectAsset.combinqations.ToList().Find(co => co.item == item).destroyItemAfterCombination)
        {
            Inventory.Instance.RemoveItem(item);
        }
        itemsEvents.Find(co => co.item == item).activationEvent.Invoke();
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
            GetComponent<Person>().Talk();
        }
    }

    public void UseItem(PointAndClickItem item)
    {
        if (itemsEvents.Find(co=>co.item == item)!=null)
        {
            FindObjectOfType<NetWalker>().GoTo(transform);
            TasksManager.Instance.SetItem(item);
            TasksManager.Instance.SetAim(this);
        }
        else
        {
            InvestigationManager.Instance.ShowDefaultWrongItemUse();
        }


    }

}
