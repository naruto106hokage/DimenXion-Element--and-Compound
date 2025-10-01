using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrollyControllerwithAngle : MonoBehaviour
{

	public static TrollyControllerwithAngle instance;
	private Animation _trainAnimation;
	private AnimationState _animState;
	public string animClipName;
	public bool isEnd;

	public Transform v1;
	public Transform v2;
	public Transform start;
	public Transform minVector;
	public Transform maxVector;

	public float minimunDis;
	public float maximunDis;

	//	public float dis;
	public float disSquare;
	//	public float minDis;
	//	public float maxDis;
	public float waitTime;
	public float velocity = 10f;
	//	public 	float angleY;
	//	public 	float playerAngleY;
	//
	public bool isFront;
	//	public float angleSum;
	float speed;

	void Start ()
	{
		instance = this;
		_trainAnimation = transform.GetComponent <Animation> ();
		_animState = _trainAnimation [animClipName];
		_trainAnimation.Play ();
		isEnd = false;
//		minDis = 12.25f;
//		maxDis = 16f;
		waitTime = .0001f;
		StartCoroutine (MoveUpdate ());
//		angleSum = 0f;
	}

	// Update is called once per frame
	//	void Update ()
	//	{
	//		dis = Vector3.Distance (v1.position, v2.position);
	//
	//		Vector3 disVector = v1.position - v2.position;
	//		disSquare = disVector.sqrMagnitude;
	//
	//		if (_animState.time < 0) {
	//			_animState.time = 0;
	//		}
	//		if (_animState.time == _animState.length) {
	//			_animState.time = _animState.length;
	//		}
	//
	//
	//
	//		float speed = Input.GetAxis ("Vertical");
	//		if (disSquare > Mathf.Pow (minDis, 2) && disSquare < Mathf.Pow (maxDis, 2)) {
	//			if (speed > 0) {
	//				_animState.speed = speed;
	//
	//			} else if (speed < 0) {
	//				_animState.speed = speed;
	//
	//			} else {
	//				_animState.speed = 0;
	//			}
	//		} else {
	//			_animState.speed = 0;
	//
	//		}


	//	}

	void FixedUpdate ()
	{
		CalculateDistance ();
		speed = Input.GetAxis ("Vertical") * velocity * Time.fixedDeltaTime;
		if (isFront) {

			CheckRaycast ();
		
		}

	}

	public void CheckRaycast ()
	{

		RaycastHit hit;

		if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000f)) {

			if (hit.collider.CompareTag ("Infront")) {

				v2.GetComponent<MovementController> ().enabled = true;

			} else {
				
				v2.GetComponent<MovementController> ().enabled = false;
			}

		}
	
	}


	IEnumerator MoveUpdate ()
	{

		while (waitTime != 0) {

			Vector3 disVector = v1.position - v2.position;
			disSquare = disVector.sqrMagnitude;

			if (_animState.time < 0) {
				_animState.time = 0;
			}
			if (_animState.time == _animState.length) {
				_animState.time = _animState.length;
			}



			//if (disSquare > Mathf.Pow (minDis, 2) && disSquare < Mathf.Pow (maxDis, 2)) {
			if (disSquare > minimunDis && disSquare < maximunDis) {
				if (speed > 0) {
					_animState.speed = speed;

				} else if (speed < 0) {
					_animState.speed = speed;

				} else {
					_animState.speed = 0;
				}
			} else {
				_animState.speed = 0;

			}
			yield return new WaitForSeconds (waitTime);

		}
		
	}


	void CalculateDistance ()
	{

		Vector3 disV1 = start.position - minVector.position;
		minimunDis = disV1.sqrMagnitude;

		Vector3 disV2 = start.position - maxVector.position;
		maximunDis = disV2.sqrMagnitude;


	}

}
