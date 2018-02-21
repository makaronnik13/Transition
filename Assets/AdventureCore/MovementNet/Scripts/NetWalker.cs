using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NetWalker : MonoBehaviour {

    public Animator animator;
    public MovementNet net;
    public float speed = 2;
    private Queue<Vector3> movementPath = new Queue<Vector3>();
    private Vector3 currentAimPoint;
    private Vector3 lastAimPoint;
    private bool moving = false;
    public Action OnFinishedPath = () => { };
    public Action OnStartedPath = () => { };

    private void OnEnable()
    {
        TasksManager.Instance.Listen(this);
    }

    private void OnDisable()
    {
        if (TasksManager.Instance)
        {
            TasksManager.Instance.StopListen(this);
        }
    }

    public void GoTo(Transform t)
    {
        MoveByPath(net.ShortestPath(transform.position, t.position));
    }

    void Awake()
    {
        currentAimPoint = net.GetNodeWorldPosition(net.GetNearestPoint(transform.position));
        lastAimPoint = currentAimPoint;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MoveByPath(net.ShortestPath(transform.position, clickPosition));
        }

        if (transform.position==currentAimPoint)
        {
            if(movementPath.Count > 0)
            {
                lastAimPoint = currentAimPoint;
                currentAimPoint = movementPath.Dequeue();
            }
            else
            {
                if (moving)
                {
                    OnFinishedPath.Invoke();
                    moving = false;
                }
            }
        }

        if (currentAimPoint!=null)
        {
            transform.localScale = net.GetScale(currentAimPoint, lastAimPoint, transform.position);
            Vector3 pos = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, currentAimPoint, Time.deltaTime * speed);
            Vector2 delta = pos - transform.position;
            animator.SetFloat("Speed", delta.normalized.magnitude);
            animator.SetFloat("X", delta.x);
            animator.SetFloat("Y", delta.y);
        }
    }

    private void MoveByPath(List<Vector3> list)
    {
        moving = true;
        OnStartedPath.Invoke();
        movementPath = new Queue<Vector3>(list);
    }
}
