using UnityEngine;
using System.Collections;

/// <summary>
/// Change depth of camera children created at runtime having gvreye component
/// </summary>
public class CameraDepthChange : MonoBehaviour
{


	public static CameraDepthChange instance;

	void Awake ()
	{
		instance = this;
	}

	void Start ()
	{


		//Invoke ("ChangeCameraValues", .0000001f);


	}

	/// <summary>
	/// Changes the camera child values.
	/// </summary>
	internal void ChangeCameraValues ()
	{
//		Camera cams = GetComponentInChildren<Camera> ();
//
//		cams.depth = 1;

	}

	public void ChangeMeshLayer (GameObject go, int index)
	{
		MeshRenderer[] mesh = go.GetComponentsInChildren<MeshRenderer> ();
		SkinnedMeshRenderer[] skinMesh = go.GetComponentsInChildren<SkinnedMeshRenderer> ();
		for (int i = 0; i < mesh.Length; i++) {

			mesh [i].gameObject.layer = index;
		}

		for (int i = 0; i < skinMesh.Length; i++) {

			skinMesh [i].gameObject.layer = index;
		}
	}
}
