using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		gameObject.transform.LookAt (dpn.DpnCameraRig._instance._center_eye.transform);
	}
}
