using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using dpn;

[RequireComponent (typeof(CharacterController))]
public class CustomMovementController : MonoBehaviour
{
	public static CustomMovementController Instance;

	public static bool IsMoving = false;

	public float speed = 4.0F;
	public float rotateSpeed = 2.0F;
	public float rotateAngle = 20;

	public bool moveForward;
	Vector3 forward;
	CharacterController controllor;
	public static float curSpeed;
	private Transform vrHead;

	public float stripOne = 1f;
	public float stripTwo = 0.3f;

	Vector2 tp;

	void Start ()
	{
		Instance = this;
		controllor = GetComponent<CharacterController> ();
		vrHead = dpn.DpnCameraRig._instance._center_eye.transform;
		IsMoving = false;

	}



	void Update ()
	{

		if (Time.timeScale == 0)
			return;

		NormalController ();
	}

	void NormalController ()
	{

		#if UNITY_EDITOR
		tp = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));

		forward = vrHead.TransformDirection(Vector3.forward);
		curSpeed = speed * tp.y;
		controllor.SimpleMove(forward * curSpeed);
		#else


		if (!DpnDaydreamController.IsTouching)
		{
		tp = Vector3.zero;
		return;
		}
		tp = DpnDaydreamController.TouchPos;
		#endif

		if (Mathf.Abs (tp.x) < Mathf.Abs (tp.y)) {
			if (tp.y < 0.5f) {
				forward = vrHead.TransformDirection (Vector3.forward);
			
				controllor.SimpleMove (forward * speed);

			} 
			//else if (tp.y < -0.5f) {
////				forward = vrHead.TransformDirection (Vector3.forward);
////				controllor.SimpleMove (forward * -speed);
//			}
		}
	}

	void OnDisable ()
	{
		IsMoving = false;
	}
}
