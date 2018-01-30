using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Person : MonoBehaviour
{

    private bool focused = false;
    public PersonObject person;

    private Transmission currentTransmission;
    public Transmission CurrentTransmission
    {
        get
        {
            if (!currentTransmission)
            {
                currentTransmission = person.Dialogs.transmissions[0];
            }
            return currentTransmission;
        }
        set
        {
            currentTransmission = value;
        }
    }


    void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject() || focused)
        {
            // we're over a UI element... peace out
            return;
        }
        focused = true;
        CursorController.Instance.SetMode(CursorController.CursorMode.LookAndTalk);
    }

    void OnMouseExit()
    {
   
        focused = false;
        CursorController.Instance.SetMode(CursorController.CursorMode.Default);
    }

    private void Update()
    {

        if (!focused)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            InvestigationManager.Instance.Invectigate(person);
            OnMouseExit();
        }

        if (Input.GetMouseButtonDown(1))
        {
            TransmissionManager.Instance.SetTalkablePerson(this);
            OnMouseExit();
        }
    }

    public void SwitchCollider()
    {
        if(GetComponent<Collider2D>().enabled)
        {
            DisableCollider();
        }
        else
        {
            EnableColider();
        }
    }

    public void DisableCollider()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    public void EnableColider()
    {
        GetComponent<Collider2D>().enabled = true;
    }
}
