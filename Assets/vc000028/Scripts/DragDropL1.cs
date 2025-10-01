using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using dpn;

public class DragDropL1 : MonoBehaviour
{

	public GameObject parentObject;
	public GameObject dragText;
	Vector3 _initialPosition;
	Quaternion _initialRotation;
	static int _dragCount = 0;


	Ray ray;
	Vector3 OriginalScale;

	bool IsDraggable = false;
	GameObject DragableObj;
	GameObject _dragDropParent;
	// Use this for initialization


	void Start ()
	{
		OriginalScale = transform.localScale;

		GetComponent <EventTrigger> ().enabled = false;
		_dragDropParent = new GameObject (gameObject.name + "_DragDropParent");
		//print (Camera.main.transform.GetComponent<GvrPointerPhysicsRaycaster> ().raycasterEventMask.value); 
	}

	void Update ()
	{
		_dragDropParent.transform.position = ReticlePointer._instance.transform.position;
	}


	public void GazeDown ()
	{
		ReticlePointer._instance.LockDistance = true;

	
		transform.parent = _dragDropParent.transform;

		_initialPosition = transform.position;
		_initialRotation = transform.rotation;
		for (int i = 0; i < 3; i++) {
			transform.GetChild (i).gameObject.layer = 12;
		} 
		transform.gameObject.layer = 12;
		GetComponent <BoxCollider> ().enabled = false;
	}

	public void GazeUp ()
	{

		RaycastHit _hit;
	
		gameObject.layer = LayerMask.NameToLayer ("Default");

		if (parentObject != null)
			transform.parent = parentObject.transform;
		else
			transform.parent = null;
		Vector3 dir = ReticlePointer._instance.transform.position -
		              dpn.DpnCameraRig._instance._center_eye.transform.position;
		if (Physics.Raycast (dpn.DpnCameraRig._instance._center_eye.transform.position, dir, out _hit, 1000f, -261)) {

			if (_hit.transform.CompareTag ("hitBeaker")) {

				_dragCount++;
				MovingBelt.instance.isRightOffsetOn = true;
				transform.localPosition = Vector3.zero;
				transform.eulerAngles = Vector3.zero;

				if (_dragCount == 1)
					Drag11 ();
				else if (_dragCount == 2)
					Drag22 ();
				else if (_dragCount == 3)
					Drag33 ();
				else if (_dragCount == 4)
					Drag44 ();
				
			} else {

				ResetOriginalPosition ();
			}
		} else {

			ResetOriginalPosition ();
		}

		//Camera.main.transform.GetComponent<GvrPointerPhysicsRaycaster> ().raycasterEventMask.value = -5; 

		for (int i = 0; i < 3; i++) {
			transform.GetChild (i).gameObject.layer = 0;
		} 

		transform.localScale = OriginalScale;
		ReticlePointer._instance.LockDistance = false;

	}

	void ResetOriginalPosition ()
	{
		transform.DOMove (_initialPosition, 0f);
		transform.DORotateQuaternion (_initialRotation, 0f);
		GetComponent <BoxCollider> ().enabled = true;

		dragText.SetActive (true); 
		transform.gameObject.layer = 0;

	}

	void Drag11 ()
	{

		GameManager.instance.StartCoroutine ("Drag1"); 
		SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [5]);

	}

	void Drag22 ()
	{

		GameManager.instance.StartCoroutine ("Drag2"); 
		SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [6]);


	}

	void Drag33 ()
	{

		GameManager.instance.StartCoroutine ("Drag3"); 
		SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [7]);

	}

	void Drag44 ()
	{

		GameManager.instance.StartCoroutine ("Drag4"); 
		SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [8]);

	}
}
