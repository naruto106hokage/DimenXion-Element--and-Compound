using UnityEngine;
using System.Collections;

/// <summary>
/// Change depth of camera children created at runtime having gvreye component
/// </summary>
public class CameraDepth : MonoBehaviour
{


	public static CameraDepth instance;

	void Awake ()
	{
		instance = this;
	}

	void Start ()
	{


		Invoke ("ChangeCameraValues", .0000001f);



	}

	/// <summary>
	/// Changes the camera child values.
	/// </summary>
	internal void ChangeCameraValues ()
	{
		Camera[] cams = GetComponentsInChildren<Camera> ();


		cams [0].depth = 1;
		//cams [2].depth = 1;

	}


	public void ChangeMeshLayer (GameObject go, int layerIndex)
	{

		gameObject.layer = layerIndex;
		MeshRenderer[] meshes = go.GetComponentsInChildren<MeshRenderer> ();

		for (int i = 0; i < meshes.Length; i++) {


			meshes [i].gameObject.layer = layerIndex;


		}

	

	}
}
