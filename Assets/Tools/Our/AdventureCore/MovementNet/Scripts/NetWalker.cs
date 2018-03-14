using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NetWalker : MonoBehaviour {

	private bool movementEnabled = true;
    public Animator animator;
    public MovementNet net;
    public float speed = 2;
    private Queue<Vector3> movementPath = new Queue<Vector3>();
    public Action OnFinishedPath = () => { };
    public Action OnStartedPath = () => { };

    private void OnEnable()
    {
        TasksManager.Instance.Listen(this);
		TransmissionManager.Instance.OnDialogFinished += DialogFinished;
		TransmissionManager.Instance.OnPersonChanged += DialogStarted;
    }

	private void DialogFinished()
	{
		movementEnabled = true;
	}

	private void DialogStarted(Person person)
	{
		movementEnabled = false;
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

    public void SetPoint(NetNode node)
    {
        transform.position = net.GetNodeWorldPosition(node);
        StartCoroutine(MoveFromTo(transform, transform.position, transform.position+Vector3.up*0.01f, speed));
        transform.localScale = net.GetScale(transform.position, transform.position, transform.position);
    }

    public void SetNet(MovementNet net)
    {
		this.net = net;
		if(net)
		{
            //transform.position = net.GetNodeWorldPosition(net.GetNearestPoint(transform.position));
            //StartCoroutine(MoveFromTo(transform, transform.position, transform.position, speed));
        }
    }

    private void Update()
    {
		if(!net)
		{
			return;
		}

		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && movementEnabled)
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MoveByPath(net.ShortestPath(transform.position, clickPosition));
        }
    }

    private void MoveByPath(List<Vector3> list)
    {
        StopCoroutine("MoveFromTo");
        OnStartedPath.Invoke();
        movementPath = new Queue<Vector3>(list);
        StartCoroutine(MoveFromTo(transform, transform.position, movementPath.Dequeue(), speed));
    }


    IEnumerator MoveFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed)
    {
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
       
        Vector2 delta =  a-b;
        animator.SetFloat("X", delta.x);
        animator.SetFloat("Y", delta.y);

        while (t <= 1.0f)
        {
            animator.SetFloat("Speed", 1);
            t += step; // Goes from 0 to 1, incrementing by step each time
            objectToMove.localScale = Vector3.Lerp(transform.localScale, net.GetScale(b, a, transform.position), Time.deltaTime*2);
            objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        objectToMove.position = b;
        animator.SetFloat("Speed", 0);

        if (movementPath.Count > 0)
        {
            StartCoroutine(MoveFromTo(transform, transform.position, movementPath.Dequeue(), speed));
        }
        else
        {
            OnFinishedPath.Invoke();
        }
    }
}
