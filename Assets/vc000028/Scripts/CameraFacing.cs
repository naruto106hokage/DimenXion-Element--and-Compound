using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacing : MonoBehaviour
{

	public Transform camY;
	public float dist = 1f;
	GameObject CamZ;
	Camera MainCam;
	// Use this for initialization
	void Awake ()
	{
		//instance=this;
		CamZ = new GameObject ("CamY");
		MainCam = dpn.DpnCameraRig._instance._center_eye;
		CamZ.transform.SetParent (MainCam.transform.parent);
		UpdateCamZ ();
	}


	void Update ()
	{
		UpdateCamZ ();

	}

	void UpdateCamZ ()
	{
		CamZ.transform.position = MainCam.transform.position;
		CamZ.transform.eulerAngles = new Vector3 (0, MainCam.transform.eulerAngles.y, 0);
	}


	void OnEnable ()
	{		
		transform.eulerAngles = new Vector3 (CamZ.transform.eulerAngles.x, CamZ.transform.eulerAngles.y, 0);
		transform.position = CamZ.transform.position + CamZ.transform.forward * 1f;
		//SetActive (true);
	}
}
	
