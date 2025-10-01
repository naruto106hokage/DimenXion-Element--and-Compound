using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using dpn;

public class DragDropTableTwo : MonoBehaviour
{
	public GameObject posOne;
	Vector3 _initialPosition;
	Quaternion _initialRotation;
	Transform initialParent;

	Ray ray;
	Vector3 OriginalScale;

	bool IsDraggable = false;
	GameObject DragableObj;

	GameObject _dragDropParent;

	void Start ()
	{
		OriginalScale = transform.localScale;
		initialParent = transform.parent;
		_dragDropParent = new GameObject (gameObject.name + "_DragDropParent");
	}

	void Update ()
	{
		_dragDropParent.transform.position = ReticlePointer._instance.transform.position;
	}

	public void OnGazeDown (int i)
	{
		
		ReticlePointer._instance.LockDistance = true;

	
		transform.parent = _dragDropParent.transform;

		GetComponent <BoxCollider> ().enabled = false;
		posOne.layer = LayerMask.NameToLayer ("DragObject");
		//	GameManagerTwo.instance.gvrRecticle.layer = LayerMask.NameToLayer ("DragObject");
		if (i == 1) {
			GameManagerTwo.instance.labels [3].SetActive (false);
			_initialPosition = gameObject.transform.position;
			_initialRotation = transform.rotation;
			GameManagerTwo.instance.cameraInInstructions [7].SetActive (false);
			posOne.transform.GetChild (0).gameObject.layer = LayerMask.NameToLayer ("DragObject");
			posOne.transform.GetChild (1).gameObject.layer = LayerMask.NameToLayer ("DragObject");

		}
		if (i == 2) {
			GameManagerTwo.instance.labels [4].SetActive (false);
			if (GameManagerTwo.instance.checkOne == false) {
				_initialPosition = gameObject.transform.position;
				_initialRotation = transform.rotation;
				GameManagerTwo.instance.beakerACollider.SetActive (true);
				GameManagerTwo.instance.cameraInInstructions [8].SetActive (false);
				posOne.transform.GetChild (0).gameObject.layer = LayerMask.NameToLayer ("DragObject");
				posOne.transform.GetChild (1).gameObject.layer = LayerMask.NameToLayer ("DragObject");
				posOne.transform.GetChild (2).gameObject.layer = LayerMask.NameToLayer ("DragObject");
			} else {

				_initialPosition = gameObject.transform.position;
				_initialRotation = transform.rotation;
				GameManagerTwo.instance.beakerBCollider.SetActive (true);
				GameManagerTwo.instance.cameraInInstructions [9].SetActive (false);
				posOne.transform.GetChild (0).gameObject.layer = LayerMask.NameToLayer ("DragObject");
				posOne.transform.GetChild (1).gameObject.layer = LayerMask.NameToLayer ("DragObject");
				posOne.transform.GetChild (2).gameObject.layer = LayerMask.NameToLayer ("DragObject");
			}


		}


	}

	public void OnMouseUp (int i)
	{
			
		RaycastHit _hit;
		gameObject.layer = LayerMask.NameToLayer ("Default");

		if (initialParent != null)
			transform.parent = initialParent.transform;
		else
			transform.parent = null;
		Vector3 dir = ReticlePointer._instance.transform.position -
		              dpn.DpnCameraRig._instance._center_eye.transform.position;
		
		posOne.layer = LayerMask.NameToLayer ("Default");
	
		if (Physics.Raycast (dpn.DpnCameraRig._instance._center_eye.transform.position, dir, out _hit, 1000f, -261)) {
			if (i == 1) {

				if (_hit.collider.gameObject.name == "CylinderCollider") {
					LanguageHandler.instance.StopVoiceOver ();
					GameManagerTwo.instance.solutionBottle.SetActive (true);
					GameManagerTwo.instance.solutionBottleCollider.SetActive (false);
					GameManagerTwo.instance.anim1.Play ("pourSolutionToCyl");
					SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [14]);
					AllCouroutineL2.instance.StartCoroutine ("AfterPourSolutionIntoCylinder");

				} else {
					gameObject.GetComponent<Collider> ().enabled = true;
					transform.DOMove (_initialPosition, 0f);
					transform.DORotateQuaternion (_initialRotation, 0f);
					GameManagerTwo.instance.labels [3].SetActive (true);
					GameManagerTwo.instance.cameraInInstructions [7].SetActive (true);
					gameObject.layer = 0;
					//GameManagerTwo.instance.gvrRecticle.layer = 0;
					posOne.transform.GetChild (1).gameObject.layer = 0;
					posOne.transform.GetChild (0).gameObject.layer = 0;

				}
			}
			if (i == 2) {

				if (GameManagerTwo.instance.checkOne == false) {

					if (_hit.collider.gameObject.name == "BeakerACollider") {
						LanguageHandler.instance.StopVoiceOver ();

						GameManagerTwo.instance.graduateCylinder.SetActive (true);
						GameManagerTwo.instance.graduateCylinderColllider.SetActive (false);
						GameManagerTwo.instance.anim1.Play ("beakerAClip");
						SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [15]);
						AllCouroutineL2.instance.StartCoroutine ("AfterPourSolutionIntoBeakerA");
						GameManagerTwo.instance.checkOne = true;

					} else {
						gameObject.GetComponent<Collider> ().enabled = true;
						transform.DOMove (_initialPosition, 0f);
						transform.DORotateQuaternion (_initialRotation, 0f);
						//transform.localPosition = new Vector3 (0, 1.25f, 0f);
						GameManagerTwo.instance.cameraInInstructions [8].SetActive (true);
						GameManagerTwo.instance.labels [4].SetActive (true);
						gameObject.layer = 0;
						//GameManagerTwo.instance.gvrRecticle.layer = 0;
						posOne.transform.GetChild (0).gameObject.layer = 0;
						posOne.transform.GetChild (1).gameObject.layer = 0;
						posOne.transform.GetChild (2).gameObject.layer = 0;
					}

				} else {

					if (_hit.collider.gameObject.name == "BeakerBCollider") {
						GameManagerTwo.instance.graduateCylinder.SetActive (true);
						GameManagerTwo.instance.graduateCylinderBlend.SetActive (false);
						GameManagerTwo.instance.graduateCylinderHalfFillColllider.SetActive (false);
						GameManagerTwo.instance.anim1.Play ("beakerBClip");
						SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [16]);
						AllCouroutineL2.instance.StartCoroutine ("AfterPourSolutionIntoBeakerB");
						GameManagerTwo.instance.checkOne = true;
						GameManagerTwo.instance.cameraInInstructions [10].SetActive (true);
					} else {
						gameObject.GetComponent<Collider> ().enabled = true;
						transform.DOMove (_initialPosition, 0f);
						transform.DORotateQuaternion (_initialRotation, 0f);
						GameManagerTwo.instance.cameraInInstructions [9].SetActive (true);
						gameObject.layer = 0;
						//GameManagerTwo.instance.gvrRecticle.layer = 0;
						GameManagerTwo.instance.labels [4].SetActive (true);
						posOne.transform.GetChild (0).gameObject.layer = 0;
						posOne.transform.GetChild (1).gameObject.layer = 0;
						posOne.transform.GetChild (2).gameObject.layer = 0;
					}

				}
			}




		} else {

			gameObject.layer = 0;
			//	GameManagerTwo.instance.gvrRecticle.layer = 0;
			gameObject.GetComponent<Collider> ().enabled = true;
			transform.DOMove (_initialPosition, 0f);
			transform.DORotateQuaternion (_initialRotation, 0f);
			if (i == 1) {
				GameManagerTwo.instance.cameraInInstructions [7].SetActive (true);
				GameManagerTwo.instance.labels [3].SetActive (true);
				posOne.transform.GetChild (1).gameObject.layer = 0;
				posOne.transform.GetChild (0).gameObject.layer = 0;
			}
			if (i == 2) {
				if (GameManagerTwo.instance.checkOne == false) {
					GameManagerTwo.instance.cameraInInstructions [8].SetActive (true);
					posOne.transform.GetChild (0).gameObject.layer = 0;
					posOne.transform.GetChild (1).gameObject.layer = 0;
					posOne.transform.GetChild (2).gameObject.layer = 0;
					GameManagerTwo.instance.labels [4].SetActive (true);
				} else {
					GameManagerTwo.instance.cameraInInstructions [9].SetActive (true);
					GameManagerTwo.instance.labels [4].SetActive (true);
				}
			}

		}
//		Camera.main.transform.parent.GetChild (1).GetComponent<GvrPointerPhysicsRaycaster> ().raycasterEventMask.value = -2053;
//		Camera.main.GetComponent<GvrPointerPhysicsRaycaster> ().raycasterEventMask.value = -2053;
		transform.localScale = OriginalScale;
		ReticlePointer._instance.LockDistance = false;

	}



}
