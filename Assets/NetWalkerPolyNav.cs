using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetWalkerPolyNav : MonoBehaviour {

	private bool movementEnabled = true;
	public Animator animator;
	//public MovementNet net;
	//public float speed = 2;
	//private Queue<Vector3> movementPath = new Queue<Vector3>();
	public Action OnFinishedPath = () => { };
	public Action OnStartedPath = () => { };
	//private bool stoping = false;

	private void OnEnable()
	{
		TasksManager.Instance.Listen(this);
		TransmissionManager.Instance.OnDialogFinished += DialogFinished;
		TransmissionManager.Instance.OnPersonChanged += DialogStarted;
		GetComponent<PolyNavAgent> ().OnPositionChanged += AnimateMovement;
		GetComponent<PolyNavAgent> ().OnSpeedChanged += AnimateSpeed;
		GetComponent<PolyNavAgent> ().SetMessageReceiver (gameObject);
	}

	void OnDestinationReached(){
		AnimateSpeed (0);
	}

	//Message from Agent
	void OnDestinationInvalid(){
		AnimateSpeed (0);
	}

	private void AnimateMovement(Vector2 delta)
	{
		Debug.Log (delta);
		animator.SetFloat("X", delta.x);
		animator.SetFloat("Y", delta.y);
	}

	private void AnimateSpeed(float speed)
	{
		animator.SetFloat("Speed", speed);
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

	/*
	public void GoTo(Transform t)
	{
		MoveByPath(net.ShortestPath(transform.position, t.position));
	}

	public void Stop()
	{
		stoping = true;
		GetComponent<NetWalker> ().StopCoroutine ("MoveFromTo");
	}

	public void Stop(Vector3 position)
	{
		Stop ();
		transform.position = position;
		transform.localScale = Vector3.Lerp(transform.localScale, net.GetScale(transform.position, transform.position, transform.position), Time.deltaTime*2);
	}


	public void SetNet(MovementNet net)
	{
		this.net = net;
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

		while (t <= 1.0f && !stoping)
		{
			animator.SetFloat("Speed", 1);
			t += step; // Goes from 0 to 1, incrementing by step each time
			if(net)
			{
				objectToMove.localScale = Vector3.Lerp(transform.localScale, net.GetScale(b, a, transform.position), Time.deltaTime*2);
			}
			objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
			yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
		}

		animator.SetFloat("Speed", 0);


		if (stoping) {
			stoping = false;
		} else {

			objectToMove.position = b;
			if (movementPath.Count > 0) {
				StartCoroutine (MoveFromTo (transform, transform.position, movementPath.Dequeue (), speed));
			} else {
				OnFinishedPath.Invoke ();
			}
		}
	}*/
}
