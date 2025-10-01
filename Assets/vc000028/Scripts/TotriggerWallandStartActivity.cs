using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotriggerWallandStartActivity : MonoBehaviour
{

	#region triggerEnter

	//	<summary> this region executes when user reaches the activity area1,activity area2... upto activity area5
	//	</summary>

	void OnTriggerEnter (Collider col)
	{
		if (col.CompareTag ("Player")) {
			GameManagerLevel3.instance.cameraInInstructions [0].SetActive (false);
			//GameManagerLevel3.instance.trooly.GetComponent<Animation> ().Stop ();
			StartCoroutine (CopperBarActivity ());
			GameManagerLevel3.instance.isTriggered = true;
			CustomMovementController.Instance.enabled = false;
		}
	}

	IEnumerator CopperBarActivity ()
	{

		if (!GameManagerLevel3.instance.isCopperActivity) {
			//yield return  new WaitForSeconds (.5f);
			GameManagerLevel3.instance.activityArea [0].SetActive (true);
	
			GameManagerLevel3.instance.activityArea [0].transform.parent.GetChild (1).GetComponent<BoxCollider> ().isTrigger = false;

			yield return new WaitForSeconds (.5f);

			GameManagerLevel3.instance.baseRotate.transform.GetComponent<ToRotateBase> ().enabled = true;
			GameManagerLevel3.instance.rays.transform.GetComponent<ToScaleRaysandRotate> ().enabled = true;
			//	yield return new WaitForSeconds (1f);
			GameManagerLevel3.instance.cameraInInstructions [1].SetActive (true);

			GameManagerLevel3.instance.rays.transform.GetChild (0).GetComponent<SkinnedMeshRenderer> ().SetBlendShapeWeight (0, 100);
			SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [19]);
			GameManagerLevel3.instance.activityAnima.Play ("copper bar appear");
			yield return new WaitForSeconds (1f);

			GameManagerLevel3.instance.activityAnima.Play ("copper bar loop");


		} else if (GameManagerLevel3.instance.isSilverActivity) {

			//print ("exxxxxxxx");
			//yield return  new WaitForSeconds (1f);
			GameManagerLevel3.instance.activityArea [1].SetActive (true);
		
			GameManagerLevel3.instance.activityArea [1].transform.parent.GetChild (1).GetComponent<BoxCollider> ().isTrigger = false;

			yield return new WaitForSeconds (.5f);

			GameManagerLevel3.instance.baseRotateSilver.transform.GetComponent<ToRotateBase> ().enabled = true;
			GameManagerLevel3.instance.raysSilver.transform.GetComponent<ToScaleRaysandRotate> ().enabled = true;
			//yield return new WaitForSeconds (1f);

			GameManagerLevel3.instance.rays.transform.GetChild (0).GetComponent<SkinnedMeshRenderer> ().SetBlendShapeWeight (0, 100);
			SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [19]);

			GameManagerLevel3.instance.AnimaitonPlay (GameManagerLevel3.instance.clip [0]);
			yield return new WaitForSeconds (1f);
			GameManagerLevel3.instance.AnimaitonPlay (GameManagerLevel3.instance.clip [1]);
			GameManagerLevel3.instance.cameraInInstructions [5].SetActive (true);

		} else if (GameManagerLevel3.instance.isMethaneActivity) {


			//yield return  new WaitForSeconds (1f);
			GameManagerLevel3.instance.activityArea [2].SetActive (true);


			GameManagerLevel3.instance.activityArea [2].transform.parent.GetChild (1).GetComponent<BoxCollider> ().isTrigger = false;

			yield return new WaitForSeconds (.5f);

			GameManagerLevel3.instance.baseRotateMethane.transform.GetComponent<ToRotateBase> ().enabled = true;
			GameManagerLevel3.instance.raysMethane.transform.GetComponent<ToScaleRaysandRotate> ().enabled = true;
			//yield return new WaitForSeconds (1f);

			GameManagerLevel3.instance.rays.transform.GetChild (0).GetComponent<SkinnedMeshRenderer> ().SetBlendShapeWeight (0, 100);
			SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [19]);

			//	yield return new WaitForSeconds (2f);

			GameManagerLevel3.instance.AnimaitonPlay (GameManagerLevel3.instance.clip [2]);
			yield return new WaitForSeconds (1f);
			GameManagerLevel3.instance.AnimaitonPlay (GameManagerLevel3.instance.clip [3]);
			GameManagerLevel3.instance.cameraInInstructions [8].SetActive (true);

		} else if (GameManagerLevel3.instance.isMedicalGasActivity) {


			//yield return  new WaitForSeconds (1f);
			GameManagerLevel3.instance.activityArea [3].SetActive (true);

			GameManagerLevel3.instance.activityArea [3].transform.parent.GetChild (1).GetComponent<BoxCollider> ().isTrigger = false;

			yield return new WaitForSeconds (.5f);

			GameManagerLevel3.instance.baseRotateMedical.transform.GetComponent<ToRotateBase> ().enabled = true;
			GameManagerLevel3.instance.raysMedical.transform.GetComponent<ToScaleRaysandRotate> ().enabled = true;
			//yield return new WaitForSeconds (1f);

			GameManagerLevel3.instance.rays.transform.GetChild (0).GetComponent<SkinnedMeshRenderer> ().SetBlendShapeWeight (0, 100);
			SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [19]);

			//	yield return new WaitForSeconds (2f);

			GameManagerLevel3.instance.AnimaitonPlay (GameManagerLevel3.instance.clip [4]);
			yield return new WaitForSeconds (1f);
			GameManagerLevel3.instance.AnimaitonPlay (GameManagerLevel3.instance.clip [5]);
			GameManagerLevel3.instance.cameraInInstructions [11].SetActive (true);

		} else if (GameManagerLevel3.instance.isWaterActivity) {
			print ("****");
			//	yield return  new WaitForSeconds (1f);
			GameManagerLevel3.instance.activityArea [4].SetActive (true);

			GameManagerLevel3.instance.activityArea [4].transform.parent.GetChild (1).GetComponent<BoxCollider> ().isTrigger = false;

			yield return new WaitForSeconds (.5f);

			GameManagerLevel3.instance.baseRotateWater.transform.GetComponent<ToRotateBase> ().enabled = true;
			GameManagerLevel3.instance.raysWater.transform.GetComponent<ToScaleRaysandRotate> ().enabled = true;
			//	yield return new WaitForSeconds (1f);

			GameManagerLevel3.instance.rays.transform.GetChild (0).GetComponent<SkinnedMeshRenderer> ().SetBlendShapeWeight (0, 100);
			SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [19]);

			//yield return new WaitForSeconds (2f);

			GameManagerLevel3.instance.AnimaitonPlay (GameManagerLevel3.instance.clip [6]);
			yield return new WaitForSeconds (1f);
			GameManagerLevel3.instance.AnimaitonPlay (GameManagerLevel3.instance.clip [7]);
			GameManagerLevel3.instance.cameraInInstructions [14].SetActive (true);

		}
	}

	#endregion
}
