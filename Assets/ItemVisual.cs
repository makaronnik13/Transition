﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemVisual : MonoBehaviour
{

    private PointAndClickItem item;

    public void Init(PointAndClickItem item)
    {
        this.item = item;
        transform.localScale = Vector3.one;
        Image itemImg = gameObject.GetComponent<Image>();
        itemImg.sprite = item.sprite;
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.offsetMin = new Vector2(5f, 5f);
        rt.offsetMax = new Vector2(-5f, -5f);
    }

    private bool focused = false;

    public void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject() || focused || !item)
        {
            // we're over a UI element... peace out
            return;
        }

        Tooltip.Instance.ShowTooltip(item.itemName);
        focused = true;
        CursorController.Instance.SetMode(InteractableObject.InteractableObjectType.Item, false);
    }

    public void OnMouseExit()
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


        if (Input.GetMouseButtonDown(1))
        {
            Click();
        }
    }

    public void Click()
    {
        if (item)
        {
            InvestigationManager.Instance.Invectigate(item);
            OnMouseExit();
        }
    }
}