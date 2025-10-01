using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragDropScript : MonoBehaviour
{
	public GameObject parentObject;
	Vector3 _initialPosition;
	Quaternion _initialRotation;
	RaycastHit _hit;
	bool check = false;

	
	public void OnGazeDown ()
	{
		//Camera.main.transform.GetComponent<PvrPointerPhysicsRaycaster> ().raycasterEventMask.value = -2053;
		if (transform.parent != Camera.main.transform)
			transform.parent = Camera.main.transform;
		_initialPosition = gameObject.transform.position;
		_initialRotation = transform.rotation;
		GetComponent <BoxCollider> ().enabled = false;
		gameObject.layer = LayerMask.NameToLayer ("DragObject");
		GameManagerTwo.instance.gvrRecticle.layer = LayerMask.NameToLayer ("DragObject");

		if (check == false) {
			GameManagerTwo.instance.WatchGlassIronSulphide.SetActive (true);
		}
		if (check == true) {
			GameManagerTwo.instance.WatchGlassMixture.SetActive (true);
		}

	}






	public void OnMouseUp ()
	{

		if (parentObject != null)
			transform.parent = parentObject.transform;
		else
			transform.parent = null;
		RaycastHit _hit;
		if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out _hit, 1000f)) {

			if (check == false) {
				if (_hit.collider.gameObject.name == "WatchGlassIronSulphide") {
					GameManagerTwo.instance.anim.Play ("forIronSulphide");
					check = true;
				} 

			}

//			if (check == true) {
//				if (_hit.collider.gameObject.name == "WatchGlassIronSulphide")
//				{
//				} 
//			}


		} else {
			gameObject.transform.DOMove (_initialPosition, 1f);
			gameObject.transform.DORotateQuaternion (_initialRotation, 1f);
			GetComponent <BoxCollider> ().enabled = true;
			gameObject.layer = LayerMask.NameToLayer ("Default");
			GameManagerTwo.instance.gvrRecticle.layer = LayerMask.NameToLayer ("Default");
		}

	}
}
