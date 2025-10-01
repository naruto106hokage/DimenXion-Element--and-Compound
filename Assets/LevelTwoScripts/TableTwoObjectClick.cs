using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableTwoObjectClick : MonoBehaviour
{

	public void ClickOnBottleCap ()
	{
		SoundManager.instance.PlayClickSound ();
		GameManagerTwo.instance.cameraInInstructions [6].SetActive (false);
		gameObject.GetComponent<BoxCollider> ().enabled = false;
		GameManagerTwo.instance.anim1.Play ("selectCap");
		SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [13]);
		AllCouroutineL2.instance.StartCoroutine ("AfterClickBottleCap");
	}
}
