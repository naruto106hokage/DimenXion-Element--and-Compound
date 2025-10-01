using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TableOneObjectClick : MonoBehaviour
{
	

	public void  OnClickMagnet ()
	{
		SoundManager.instance.PlayClickSound ();
		if (GameManagerTwo.instance.check == false) {
			GameManagerTwo.instance.cameraInInstructions [2].SetActive (false);
			GameManagerTwo.instance.magnet.GetComponent<BoxCollider> ().enabled = false;
			GameManagerTwo.instance.labels [0].SetActive (false);
			GameManagerTwo.instance.labels [1].SetActive (false);
			GameManagerTwo.instance.anim.Play ("forIronSulphide");
			SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [10]);
			GameManagerTwo.instance.check = true;
			AllCouroutineL2.instance.StartCoroutine ("AfterIronMetal");

		} else {
			
			GameManagerTwo.instance.cameraInInstructions [3].SetActive (false);
			GameManagerTwo.instance.magnet.GetComponent<BoxCollider> ().enabled = false;
			GameManagerTwo.instance.labels [0].SetActive (false);
			GameManagerTwo.instance.labels [2].SetActive (false);
			GameManagerTwo.instance.anim.Play ("forIronMixture");
			SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [11]);
			Invoke ("ShowProcessInst", 3.5f);
			AllCouroutineL2.instance.StartCoroutine ("AfterIronMixtureMetal");
		}
	}


	void ShowProcessInst ()
	{
		GameManagerTwo.instance.cameraInInstructions [4].SetActive (true);
	}



}
