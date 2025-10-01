using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using dpn;

public class TrollyController : MonoBehaviour
{

	public static TrollyController instance;
	private Animation _trainAnimation;
	private AnimationState _animState;
	public string animClipName;
	public bool isEnd;

	public Transform v1;
	public Transform v2;
	public float dis;

	public float minDis;
	public float maxDis;

	public bool isTrainEnable = false;
	Vector2 tp;


	/// <summary>
	/// this script is used to get vertical input from user to controlle the trolly animation/movement 
	/// </summary>

	void Start ()
	{
		instance = this;
		_trainAnimation = transform.GetComponent <Animation> ();
		_animState = _trainAnimation [animClipName];
		_trainAnimation.Play ();
		isEnd = false;
	
	}


	void Update ()
	{
		
		dis = Vector3.Distance (v1.position, v2.position);

	
		if (_animState.time < 0) {
			_animState.time = 0;
		}
		if (_animState.time == _animState.length) {
			_animState.time = _animState.length;
		}

		if (dis < minDis) {
			isTrainEnable = true;
		} else
			isTrainEnable = false;
		

		#if UNITY_EDITOR
		tp = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));

		#else
		tp = DpnDaydreamController.TouchPos;
		#endif

		float speed = tp.y;

		if (tp.y < 0.5 && isTrainEnable) {

			if (dis < minDis) {
				
				_animState.speed = 0.6f;

			} else if (dis == minDis) {
				_animState.speed = 0;

			} else {
				_animState.speed = 0.3f;
				//StartCoroutine ("ResetAnim");	
			}

		} else {
			_animState.speed = 0;
		}
	}




	IEnumerator ResetAnim ()
	{
		yield return new WaitForSeconds (Time.deltaTime);
		_animState.speed = 0;
	}
}
