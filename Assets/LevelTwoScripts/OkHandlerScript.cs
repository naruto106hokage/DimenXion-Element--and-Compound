using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OkHandlerScript : MonoBehaviour
{
	public static OkHandlerScript instance;

	void Start ()
	{
		instance = this;

	}

	public void FollowInstOkButton ()
	{



		GameManagerTwo.instance.anim.Play ("zoomClip_d");
		AllCouroutineL2.instance.StartCoroutine ("AfterMetalPlacedOnTable");
		GameManagerTwo.instance.cameraInInstructions [11].SetActive (true);

	}



	//	void PlayAnimation()
	//	{
	//
	//		GameManagerTwo.instance.anim.Play ("zoomClip_d");
	//		AllCouroutineL2.instance.StartCoroutine ("AfterMetalPlacedOnTable");
	//	}

}
