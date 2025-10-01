using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ColliderForVRPlayer : MonoBehaviour
{


	void OnTriggerEnter (Collider other)
	{

		if (other.gameObject.name == "HighlightedAreaOne") {
			GameManagerTwo.instance.HighlightedAreaOne.SetActive (false);
			GameManagerTwo.instance.ActivityColliderOne.SetActive (true);
			GameManagerTwo.instance.labels [0].SetActive (true);
			GameManagerTwo.instance.labels [1].SetActive (true);
			GameManagerTwo.instance.labels [2].SetActive (true);
			GameManagerTwo.instance.cameraInInstructions [0].SetActive (false);
			GameManagerTwo.instance.instructions [0].SetActive (true);


		}


		if (other.gameObject.name == "HighlightedAreaTwo") {
			GameManagerTwo.instance.HighlightedAreaTwo.SetActive (false);
			GameManagerTwo.instance.ActivityColliderTwo.SetActive (true);
			GameManagerTwo.instance.labels [3].SetActive (true);
			GameManagerTwo.instance.labels [4].SetActive (true);
			GameManagerTwo.instance.labels [5].SetActive (true);
			GameManagerTwo.instance.labels [6].SetActive (true);
			GameManagerTwo.instance.cameraInInstructions [5].SetActive (false);
			GameManagerTwo.instance.cameraInInstructions [6].SetActive (true);
			GameManagerTwo.instance.tableScreen.transform.DOLocalMoveZ (3.99f, 3f);
			SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [12]);

			GameManagerTwo.instance.bottleCap.GetComponent<BoxCollider> ().enabled = true;


		}
	}
}
