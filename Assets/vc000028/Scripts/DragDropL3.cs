using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using DG.Tweening;
using dpn;

public class DragDropL3 : MonoBehaviour
{

	public GameObject parentObject;
	Vector3 _initialPosition;
	Quaternion _initialRotation;
	//static int _dragCount = 0;

	Ray ray;
	Vector3 OriginalScale;

	bool IsDraggable = false;
	GameObject DragableObj;
	GameObject _dragDropParent;

	void Start ()
	{
		OriginalScale = transform.localScale;
		_dragDropParent = new GameObject (gameObject.name + "_DragDropParent");
	}

	void Update ()
	{
		_dragDropParent.transform.position = ReticlePointer._instance.transform.position;
	}

	/// <summary>
	/// this script is used for drag and drop of level3 objects and also get the score when user successfully drop it into correct box.
	/// </summary>
	public void GazeDown ()
	{
		ReticlePointer._instance.LockDistance = true;

	
		transform.parent = _dragDropParent.transform;
			
		_initialPosition = transform.position;
		_initialRotation = transform.rotation;

		transform.GetComponent <Collider> ().enabled = false;

		transform.gameObject.layer = 12;
		//GameManagerLevel3.instance.gvrRectical.layer = 12;
		//MovementController.Instance.enabled = false;
	}

	public void GazeUp ()
	{

		RaycastHit _hit;
		// For Daydream

			
		transform.DOScale (Vector3.one, 0f);
		transform.gameObject.layer = 0;
		//GameManagerLevel3.instance.gvrRectical.layer = 0;
		//	MovementController.Instance.enabled = true;
		gameObject.layer = LayerMask.NameToLayer ("Default");

		if (parentObject != null)
			transform.parent = parentObject.transform;
		else
			transform.parent = null;
		Vector3 dir = ReticlePointer._instance.transform.position -
		              dpn.DpnCameraRig._instance._center_eye.transform.position;
		if (Physics.Raycast (dpn.DpnCameraRig._instance._center_eye.transform.position, dir, out _hit, 1000f, -261)) {
			
			if (transform.CompareTag ("PureSubstance")) {


				if (_hit.collider.tag == transform.gameObject.tag) {
					GameManagerLevel3.instance.dragCount++;
					//	print ("_dragCount" + _dragCount);

					if (GameManagerLevel3.instance.dragCount == 1) {
						StartCoroutine (GameManagerLevel3.instance.DragCopper (_hit, transform.gameObject));

						GameManagerLevel3.instance.wellDone.SetActive (true);
						GameManagerLevel3.instance.greenPuresubstance.SetActive (true);
						ScoreManager.instance.ScoreCount ("Q1");
					
					} else if (GameManagerLevel3.instance.dragCount == 3) {
						StartCoroutine (GameManagerLevel3.instance.DragCylinder (_hit, transform.gameObject));
						GameManagerLevel3.instance.wellDone.SetActive (true);
						GameManagerLevel3.instance.greenPuresubstance.SetActive (true);

						ScoreManager.instance.ScoreCount ("Q3");
					
					}

				} else {

					ResetOriginalPosition ();
					if (_hit.collider.tag == "Mixture") {
						GameManagerLevel3.instance.dragCount++;
						//print ("_dragCount" + _dragCount);

						transform.GetComponent<Collider> ().enabled = false;
						ScoreManager.instance.WrongCount++;
						if (GameManagerLevel3.instance.dragCount == 1) {
							ScoreManager.instance.ScoreCount ("Q1");
						
							GameManagerLevel3.instance.isPlacedCopper = true;
							GameManagerLevel3.instance.cameraInInstructions [4].SetActive (false);

							GameManagerLevel3.instance.tryAgain.SetActive (true);
							StartCoroutine ("EnableDisabletryAgain");
							//	Invoke ("Autocorrect", 3f);
						} else if (GameManagerLevel3.instance.dragCount == 3) {
							ScoreManager.instance.ScoreCount ("Q3");
						
							GameManagerLevel3.instance.isPlacedCylinder = true;
							GameManagerLevel3.instance.cameraInInstructions [10].SetActive (false);

							GameManagerLevel3.instance.tryAgain.SetActive (true);
							StartCoroutine ("EnableDisabletryAgain");
							//	Invoke ("Autocorrect", 3f);
						}


					}

				
				}


			} else if (transform.CompareTag ("Mixture")) {

		
				if (_hit.collider.tag == transform.gameObject.tag) {
					GameManagerLevel3.instance.dragCount++;
			
					//	print ("_dragCount" + _dragCount);

					if (GameManagerLevel3.instance.dragCount == 2) {
						StartCoroutine (GameManagerLevel3.instance.DragSilver (_hit, transform.gameObject));
					
						GameManagerLevel3.instance.wellDone.SetActive (true);
						GameManagerLevel3.instance.greenMixture.SetActive (true);

						ScoreManager.instance.ScoreCount ("Q2");
					
					} else if (GameManagerLevel3.instance.dragCount == 4) {
						
						StartCoroutine (GameManagerLevel3.instance.DragMedicalGas (_hit, transform.gameObject));

						GameManagerLevel3.instance.wellDone.SetActive (true);
						GameManagerLevel3.instance.greenMixture.SetActive (true);

						ScoreManager.instance.ScoreCount ("Q4");

					} else if (GameManagerLevel3.instance.dragCount == 5) {

						StartCoroutine (GameManagerLevel3.instance.DragSalineWater (_hit, transform.gameObject));
					
						GameManagerLevel3.instance.wellDone.SetActive (true);
						GameManagerLevel3.instance.greenMixture.SetActive (true);

						ScoreManager.instance.ScoreCount ("Q5");
					}
				} else {

					ResetOriginalPosition ();
					if (_hit.collider.tag == "PureSubstance") {

						GameManagerLevel3.instance.dragCount++;
						//print ("_dragCount" + _dragCount);

						transform.GetComponent<Collider> ().enabled = false;
						ScoreManager.instance.WrongCount++;
						if (GameManagerLevel3.instance.dragCount == 2) {
							ScoreManager.instance.ScoreCount ("Q2");
							GameManagerLevel3.instance.isPlacedSilver = true;
							GameManagerLevel3.instance.cameraInInstructions [7].SetActive (false);

							GameManagerLevel3.instance.tryAgain.SetActive (true);
							StartCoroutine ("EnableDisabletryAgain");
							//Invoke ("Autocorrect", 3f);

						} else if (GameManagerLevel3.instance.dragCount == 4) {
							ScoreManager.instance.ScoreCount ("Q4");
						
							GameManagerLevel3.instance.isPlacedMedical = true;
							GameManagerLevel3.instance.cameraInInstructions [13].SetActive (false);

							GameManagerLevel3.instance.tryAgain.SetActive (true);
							StartCoroutine ("EnableDisabletryAgain");
							//Invoke ("Autocorrect", 3f);
							
						} else if (GameManagerLevel3.instance.dragCount == 5) {

							ScoreManager.instance.ScoreCount ("Q5");

							GameManagerLevel3.instance.isPlacedWater = true;
							GameManagerLevel3.instance.cameraInInstructions [16].SetActive (false);

							GameManagerLevel3.instance.tryAgain.SetActive (true);
							StartCoroutine ("EnableDisabletryAgain");
							//Invoke ("Autocorrect", 3f);
						}
					}

				}

			} else {

				ResetOriginalPosition ();
				
			}
				
		
		} else {

			ResetOriginalPosition ();
			print ("not hit...");

		}
		transform.localScale = OriginalScale;
		ReticlePointer._instance.LockDistance = false;
	
	}

	void ResetOriginalPosition ()
	{
		transform.DOScale (Vector3.one, 0f);
		transform.gameObject.layer = 0;
		transform.DOMove (_initialPosition, 0f);
		transform.DORotateQuaternion (_initialRotation, 0f);
		transform.GetComponent <Collider> ().enabled = true;
	}

	IEnumerator EnableDisabletryAgain ()
	{
		//GameManagerLevel3.instance.tryAgain.SetActive (true);
		yield return new WaitForSeconds (2f);
		GameManagerLevel3.instance.tryAgain.SetActive (false);

		StartCoroutine ("Autocorrect");

	}


	//	IEnumerator CorrectResponseVo ()
	//	{
	//		LanguageHandler.instance.PlayVoiceOver ("C_Inst_RightOption");
	//		yield return new WaitForSeconds (LanguageHandler.instance.GetLength ("C_Inst_RightOption").length);
	//		Invoke ("Autocorrect", .2f);
	//	}

	IEnumerator Autocorrect ()
	{
		LanguageHandler.instance.PlayVoiceOver ("C_Correct_Response");
	

		if (GameManagerLevel3.instance.isPlacedCopper) {
			GameManagerLevel3.instance.isPlacedCopper = false;
			//transform.DOMove (GameManagerLevel3.instance.target1.transform.position, 1f);
			transform.DOJump (GameManagerLevel3.instance.target1.transform.position, 1f, 1, 1f);
			GameManagerLevel3.instance.greenPuresubstance.SetActive (true);

			transform.DOScale (new Vector3 (.4f, .4f, .4f), .2f);
			transform.SetParent (GameManagerLevel3.instance.target1.transform);
			float len = LanguageHandler.instance.GetLength ("C_Correct_Response").length;
			yield return new WaitForSeconds (len);
			GameManagerLevel3.instance.afterPlacedmsg [0].SetActive (true);
		
		} else if (GameManagerLevel3.instance.isPlacedSilver) {
			GameManagerLevel3.instance.isPlacedSilver = false;

			//transform.DOMove (GameManagerLevel3.instance.target2.transform.position, 1f);
			transform.DOJump (GameManagerLevel3.instance.target2.transform.position, 1f, 1, 1f);
			GameManagerLevel3.instance.greenMixture.SetActive (true);

			transform.DOScale (new Vector3 (.4f, .4f, .4f), .2f);
			transform.SetParent (GameManagerLevel3.instance.target2.transform);
			float len = LanguageHandler.instance.GetLength ("C_Correct_Response").length;
			yield return new WaitForSeconds (len);

			GameManagerLevel3.instance.afterPlacedmsg [1].SetActive (true);
		
		} else if (GameManagerLevel3.instance.isPlacedCylinder) {
			GameManagerLevel3.instance.isPlacedCylinder = false;

			//	transform.DOMove (GameManagerLevel3.instance.target1.transform.position, 1f);
			transform.DOJump (GameManagerLevel3.instance.target1.transform.position, 1f, 1, 1f);
			GameManagerLevel3.instance.greenPuresubstance.SetActive (true);

			transform.DOScale (new Vector3 (.4f, .4f, .4f), .2f);
			transform.SetParent (GameManagerLevel3.instance.target1.transform);
			float len = LanguageHandler.instance.GetLength ("C_Correct_Response").length;
			yield return new WaitForSeconds (len);

			GameManagerLevel3.instance.afterPlacedmsg [2].SetActive (true);
		

		} else if (GameManagerLevel3.instance.isPlacedMedical) {
			GameManagerLevel3.instance.isPlacedMedical = false;

			//	transform.DOMove (GameManagerLevel3.instance.target2.transform.position, 1f);
			transform.DOJump (GameManagerLevel3.instance.target2.transform.position, 1f, 1, 1f);
			GameManagerLevel3.instance.greenMixture.SetActive (true);

			transform.DOScale (new Vector3 (.4f, .4f, .4f), .2f);
			transform.SetParent (GameManagerLevel3.instance.target2.transform);
			float len = LanguageHandler.instance.GetLength ("C_Correct_Response").length;
			yield return new WaitForSeconds (len);

			GameManagerLevel3.instance.afterPlacedmsg [3].SetActive (true);
		

		} else if (GameManagerLevel3.instance.isPlacedWater) {
			GameManagerLevel3.instance.isPlacedWater = false;
			CustomMovementController.Instance.enabled = false;

			//transform.DOMove (GameManagerLevel3.instance.target2.transform.position, 1f);
			transform.DOJump (GameManagerLevel3.instance.target2.transform.position, 1f, 1, 1f);
			GameManagerLevel3.instance.greenMixture.SetActive (true);

			transform.DOScale (new Vector3 (.4f, .4f, .4f), .2f);
			transform.SetParent (GameManagerLevel3.instance.target2.transform);
			float len = LanguageHandler.instance.GetLength ("C_Correct_Response").length;
			yield return new WaitForSeconds (len);

			GameManagerLevel3.instance.afterPlacedmsg [4].SetActive (true);
		

		}
	}
}
