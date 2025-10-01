using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacing2 : MonoBehaviour
{
        public static CameraFacing2 instance;
//	public Transform camY;
//	public float dist = 1f;
	GameObject CamZ;
	Camera MainCam;
	// Use this for initialization
	void Awake ()
	{
		instance=this;
		CamZ = new GameObject("CamY");
		MainCam = dpn.DpnCameraRig._instance._center_eye;
		CamZ.transform.SetParent(MainCam.transform.parent);
		UpdateCamZ();
	}


	void Update()
	{
		 UpdateCamZ();

	}

	void UpdateCamZ()
	{
		CamZ.transform.position = MainCam.transform.position;
		CamZ.transform.eulerAngles = new Vector3(0,MainCam.transform.eulerAngles.y,0);
	}


	public void DisplayPanel(GameObject go)
	{
		go.transform.eulerAngles = new Vector3(MainCam.transform.eulerAngles.x, MainCam.transform.eulerAngles.y, 0);
		go.transform.position = MainCam.transform.position + MainCam.transform.forward * 1f;
		go.SetActive (true);
	}
}
