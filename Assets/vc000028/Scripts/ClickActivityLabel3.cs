using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ClickActivityLabel3 : MonoBehaviour
{

	public void OnClickScoreInst ()
	{
		//SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [9]);

	}

	public void OnClickLabel3Inst1 ()
	{
		SoundManager.instance.PlayClickSound ();
		TrollyController.instance.isTrainEnable = true;
		GameManagerLevel3.instance.cameraInInstructions [0].SetActive (true);
		//TrollyControllerwithAngle.instance.isFront = true;
	}

	#region Activity1

	//	<summary> this region is for the actvity1(CopperActivity)
	//	</summary>
	public void OnClickCopperBar ()
	{
		SoundManager.instance.PlayClickSound ();
		DisableLables ();
		GameManagerLevel3.instance.copperBar.enabled = false;
		GameManagerLevel3.instance.cameraInInstructions [1].SetActive (false);

		GameManagerLevel3.instance.toChangeAlphaValue.DOColor (new Color32 (255, 255, 255, 255), 1.5f);
		SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [20]);

		StartCoroutine (EnableCopperSkybox ());
	}



	IEnumerator EnableCopperSkybox ()
	{
		yield return new WaitForSeconds (1.6f);
		GameManagerLevel3.instance.toChangeAlphaValue.transform.DOScale (Vector3.zero, 0f);

		GameManagerLevel3.instance.lab.transform.DOScale (Vector3.zero, 0f);

		GameManagerLevel3.instance.allSkyBox [0].SetActive (true);
		GameManagerLevel3.instance.cameraInInstructions [2].SetActive (true);

		yield return new WaitForSeconds (4f);
		GameManagerLevel3.instance.cameraInInstructions [2].SetActive (false);
		GameManagerLevel3.instance.cameraInInstructions [3].SetActive (true);
		GameManagerLevel3.instance.btnBack.SetActive (true);
//		GameManagerLevel3.instance.btnBack.transform.parent = dpn.DpnCameraRig._instance._center_eye.transform.parent.parent;
		GameManagerLevel3.instance.copperBar.enabled = true;

	}

	public void OnClickWellDoneCopper ()
	{
		SoundManager.instance.PlayClickSound ();
		//TrollyControllerwithAngle.instance.isFront = true;

		GameManagerLevel3.instance.isCopperActivity = true;
		GameManagerLevel3.instance.isSilverActivity = true;
		GameManagerLevel3.instance.isTriggered = false;
		GameManagerLevel3.instance.afterPlacedmsg [0].SetActive (false);
		GameManagerLevel3.instance.cameraInInstructions [0].SetActive (true);
		GameManagerLevel3.instance.activityArea [0].SetActive (false);

		EventTriggerManager.instance.SetEventTriggerState (GameManagerLevel3.instance.copperBar, EventTriggerType.PointerUp, "GazeUp", UnityEventCallState.Off);
		EventTriggerManager.instance.SetEventTriggerState (GameManagerLevel3.instance.copperBar, EventTriggerType.PointerDown, "GazeDown", UnityEventCallState.Off);

		GameManagerLevel3.instance.baseRotate.transform.GetComponent<ToRotateBase> ().enabled = false;
		GameManagerLevel3.instance.rays.SetActive (false);
		GameManagerLevel3.instance.activityArea [0].SetActive (false);
		//Invoke ("DelayBoolSilver", 1f);
	}

	#endregion

	#region BackButton

	//	<summary>  Back button function it execute for all the activity of level3 when back button is clicked
	//	</summary>
	public void OnClickBackButton ()
	{
		SoundManager.instance.PlayClickSound ();
		GameManagerLevel3.instance.toChangeAlphaValue.DOColor (new Color32 (255, 255, 255, 0), 0f);

		GameManagerLevel3.instance.cameraInInstructions [3].SetActive (false);
		GameManagerLevel3.instance.allSkyBox [0].SetActive (false);
		GameManagerLevel3.instance.allSkyBox [1].SetActive (false);
		GameManagerLevel3.instance.allSkyBox [2].SetActive (false);
		GameManagerLevel3.instance.allSkyBox [3].SetActive (false);
		GameManagerLevel3.instance.allSkyBox [4].SetActive (false);

		GameManagerLevel3.instance.btnBack.SetActive (false);
		GameManagerLevel3.instance.toChangeAlphaValue.transform.DOScale (Vector3.one, 0f);

		GameManagerLevel3.instance.toChangeAlphaValue.DOColor (new Color32 (255, 255, 255, 255), 1.5f);
		SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [20]);

		Invoke ("ImageOnOff", 1.4f);

	
		if (!GameManagerLevel3.instance.isCopperActivity) {

			Invoke ("CopperActivity", 1.5f);
		} else if (GameManagerLevel3.instance.isSilverActivity) {
			Invoke ("SilverActivity", 1.5f);

		} else if (GameManagerLevel3.instance.isMethaneActivity) {
			
			Invoke ("MethaneActivity", 1.5f);

		} else if (GameManagerLevel3.instance.isMedicalGasActivity) {
			Invoke ("MedicalGasActivity", 1.5f);

		} else if (GameManagerLevel3.instance.isWaterActivity) {
			
			Invoke ("SalineWaterActivity", 1.5f);

		}

	}

	#endregion


	void CopperActivity ()     // invoked for the copper activity
	{
		EnableLables ();

		GameManagerLevel3.instance.lab.transform.DOScale (Vector3.one, 0f);

		GameManagerLevel3.instance.cameraInInstructions [4].SetActive (true);

		EventTriggerManager.instance.SetEventTriggerState (GameManagerLevel3.instance.copperBar, EventTriggerType.PointerClick, "OnClickCopperBar", UnityEventCallState.Off);

		EventTriggerManager.instance.SetEventTriggerState (GameManagerLevel3.instance.copperBar, EventTriggerType.PointerUp, "GazeUp", UnityEventCallState.RuntimeOnly);
		EventTriggerManager.instance.SetEventTriggerState (GameManagerLevel3.instance.copperBar, EventTriggerType.PointerDown, "GazeDown", UnityEventCallState.RuntimeOnly);
	}

	void SilverActivity ()    // invoked for the silver activity
	{
		EnableLables ();

		GameManagerLevel3.instance.lab.transform.DOScale (Vector3.one, 0f);

		GameManagerLevel3.instance.cameraInInstructions [7].SetActive (true);
		//GameManagerLevel3.instance.isPlacedSilver = true;
		EventTriggerManager.instance.SetEventTriggerState (GameManagerLevel3.instance.silverBar, EventTriggerType.PointerClick, "OnClickSilverBar", UnityEventCallState.Off);

		EventTriggerManager.instance.SetEventTriggerState (GameManagerLevel3.instance.silverBar, EventTriggerType.PointerUp, "GazeUp", UnityEventCallState.RuntimeOnly);
		EventTriggerManager.instance.SetEventTriggerState (GameManagerLevel3.instance.silverBar, EventTriggerType.PointerDown, "GazeDown", UnityEventCallState.RuntimeOnly);
	}

	void MethaneActivity ()     // invoked for the methane activity
	{
		EnableLables ();
		GameManagerLevel3.instance.allSkyBox [2].SetActive (false);

		GameManagerLevel3.instance.lab.transform.DOScale (Vector3.one, 0f);

		GameManagerLevel3.instance.cameraInInstructions [10].SetActive (true);
		//GameManagerLevel3.instance.isPlacedCylinder = true;
		EventTriggerManager.instance.SetEventTriggerState (GameManagerLevel3.instance.cylinderMethane, EventTriggerType.PointerClick, "OnClickCylinder", UnityEventCallState.Off);

		EventTriggerManager.instance.SetEventTriggerState (GameManagerLevel3.instance.cylinderMethane, EventTriggerType.PointerUp, "GazeUp", UnityEventCallState.RuntimeOnly);
		EventTriggerManager.instance.SetEventTriggerState (GameManagerLevel3.instance.cylinderMethane, EventTriggerType.PointerDown, "GazeDown", UnityEventCallState.RuntimeOnly);
	}

	void MedicalGasActivity ()    // invoked for the medical gas activity
	{
		EnableLables ();
		GameManagerLevel3.instance.allSkyBox [3].SetActive (false);

		GameManagerLevel3.instance.lab.transform.DOScale (Vector3.one, 0f);

		GameManagerLevel3.instance.cameraInInstructions [13].SetActive (true);
		//GameManagerLevel3.instance.isPlacedMedical = true;
		EventTriggerManager.instance.SetEventTriggerState (GameManagerLevel3.instance.medicalGas, EventTriggerType.PointerClick, "OnClickMedicalGas", UnityEventCallState.Off);

		EventTriggerManager.instance.SetEventTriggerState (GameManagerLevel3.instance.medicalGas, EventTriggerType.PointerUp, "GazeUp", UnityEventCallState.RuntimeOnly);
		EventTriggerManager.instance.SetEventTriggerState (GameManagerLevel3.instance.medicalGas, EventTriggerType.PointerDown, "GazeDown", UnityEventCallState.RuntimeOnly);

	}

	void SalineWaterActivity ()     // invoked for the saline water activity
	{
		EnableLables ();
		GameManagerLevel3.instance.isWaterActivity = false;

		GameManagerLevel3.instance.allSkyBox [4].SetActive (false);

		GameManagerLevel3.instance.lab.transform.DOScale (Vector3.one, 0f);
		GameManagerLevel3.instance.cameraInInstructions [14].SetActive (false);

		GameManagerLevel3.instance.cameraInInstructions [16].SetActive (true);
		//GameManagerLevel3.instance.isPlacedWater = true;
		EventTriggerManager.instance.SetEventTriggerState (GameManagerLevel3.instance.waterGas, EventTriggerType.PointerClick, "OnClickWaterGas", UnityEventCallState.Off);

		EventTriggerManager.instance.SetEventTriggerState (GameManagerLevel3.instance.waterGas, EventTriggerType.PointerUp, "GazeUp", UnityEventCallState.RuntimeOnly);
		EventTriggerManager.instance.SetEventTriggerState (GameManagerLevel3.instance.waterGas, EventTriggerType.PointerDown, "GazeDown", UnityEventCallState.RuntimeOnly);
	}

	void ImageOnOff ()
	{
		GameManagerLevel3.instance.toChangeAlphaValue.transform.DOScale (Vector3.zero, 0f);

	}



	void DelayBoolSilver ()
	{
		GameManagerLevel3.instance.isSilverActivity = true;

	}

	#region Activity2

	//	<summary> this region is for the actvity2(SilverActivity)
	//	</summary>
	public void OnClickSilverBar ()
	{
		SoundManager.instance.PlayClickSound ();
		DisableLables ();
		GameManagerLevel3.instance.silverBar.enabled = false;

		GameManagerLevel3.instance.cameraInInstructions [5].SetActive (false);

		GameManagerLevel3.instance.toChangeAlphaValue.transform.DOScale (Vector3.one, 0f);

		GameManagerLevel3.instance.toChangeAlphaValue.DOColor (new Color32 (255, 255, 255, 255), 1.5f);
		SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [20]);

		StartCoroutine (EnableSilverSkybox ());
	}

	IEnumerator EnableSilverSkybox ()
	{
		yield return new WaitForSeconds (1.6f);
		GameManagerLevel3.instance.toChangeAlphaValue.transform.DOScale (Vector3.zero, 0f);
	
		GameManagerLevel3.instance.lab.transform.DOScale (Vector3.zero, 0f);
		GameManagerLevel3.instance.cameraInInstructions [6].SetActive (true);
		GameManagerLevel3.instance.allSkyBox [1].SetActive (true);
		yield return new WaitForSeconds (4f);
		GameManagerLevel3.instance.cameraInInstructions [6].SetActive (false);

		GameManagerLevel3.instance.cameraInInstructions [3].SetActive (true);
		GameManagerLevel3.instance.btnBack.SetActive (true);
		//CameraFacing2.instance.DisplayPanel (GameManagerLevel3.instance.btnBack);
		//	GameManagerLevel3.instance.btnBack.transform.parent = dpn.DpnCameraRig._instance._center_eye.transform.parent.parent;
		GameManagerLevel3.instance.silverBar.enabled = true;

	}

	public void OnClickWellDoneSilver ()
	{
		SoundManager.instance.PlayClickSound ();
		//TrollyControllerwithAngle.instance.isFront = true;

		GameManagerLevel3.instance.isSilverActivity = false;
		GameManagerLevel3.instance.isMethaneActivity = true;
		GameManagerLevel3.instance.afterPlacedmsg [1].SetActive (false);
		GameManagerLevel3.instance.cameraInInstructions [0].SetActive (true);
		GameManagerLevel3.instance.activityArea [1].SetActive (false);
		//GameManagerLevel3.instance.offActivity2.SetActive (false);
		GameManagerLevel3.instance.baseRotateSilver.transform.GetComponent<ToRotateBase> ().enabled = false;
		GameManagerLevel3.instance.raysSilver.SetActive (false);
	}

	#endregion


	#region Activity3

	//	<summary> this region is for the actvity3(methaneActivity)
	//	</summary>
	public void OnClickCylinder ()
	{
		SoundManager.instance.PlayClickSound ();
		DisableLables ();
		GameManagerLevel3.instance.cameraInInstructions [8].SetActive (false);
		GameManagerLevel3.instance.toChangeAlphaValue.transform.DOScale (Vector3.one, 0f);

		GameManagerLevel3.instance.toChangeAlphaValue.DOColor (new Color32 (255, 255, 255, 255), 1.5f);
		SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [20]);
	
		StartCoroutine (EnableCylinderSkybox ());
		GameManagerLevel3.instance.cylinderMethane.enabled = false;

	}

	IEnumerator EnableCylinderSkybox ()
	{
		yield return new WaitForSeconds (1.6f);
		GameManagerLevel3.instance.toChangeAlphaValue.transform.DOScale (Vector3.zero, 0f);
	
		GameManagerLevel3.instance.lab.transform.DOScale (Vector3.zero, 0f);
		GameManagerLevel3.instance.cameraInInstructions [9].SetActive (true);

		GameManagerLevel3.instance.allSkyBox [2].SetActive (true);
		yield return new WaitForSeconds (4f);
		GameManagerLevel3.instance.cameraInInstructions [9].SetActive (false);

		GameManagerLevel3.instance.cameraInInstructions [3].SetActive (true);
		GameManagerLevel3.instance.btnBack.SetActive (true);
		//GameManagerLevel3.instance.btnBack.transform.parent = dpn.DpnCameraRig._instance._center_eye.transform.parent.parent;
		GameManagerLevel3.instance.cylinderMethane.enabled = true;

	}

	public void OnClickWellDoneCylinder ()
	{
		SoundManager.instance.PlayClickSound ();
		//TrollyControllerwithAngle.instance.isFront = true;

		GameManagerLevel3.instance.isMethaneActivity = false;
		GameManagerLevel3.instance.isMedicalGasActivity = true;
		GameManagerLevel3.instance.afterPlacedmsg [2].SetActive (false);
		GameManagerLevel3.instance.cameraInInstructions [0].SetActive (true);
		GameManagerLevel3.instance.activityArea [2].SetActive (false);
		GameManagerLevel3.instance.baseRotateMethane.transform.GetComponent<ToRotateBase> ().enabled = false;
		GameManagerLevel3.instance.raysMethane.SetActive (false);
	}

	#endregion


	#region Activity4

	//	<summary> this region is for the actvity4(medicalgasActivity)
	//	</summary>
	public void OnClickMedicalGas ()
	{
		SoundManager.instance.PlayClickSound ();
		DisableLables ();
		GameManagerLevel3.instance.cameraInInstructions [11].SetActive (false);
		GameManagerLevel3.instance.toChangeAlphaValue.transform.DOScale (Vector3.one, 0f);

		GameManagerLevel3.instance.toChangeAlphaValue.DOColor (new Color32 (255, 255, 255, 255), 1.5f);
		SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [20]);

		StartCoroutine (EnableMedicaGasSkybox ());
		GameManagerLevel3.instance.medicalGas.enabled = false;

	}

	IEnumerator EnableMedicaGasSkybox ()
	{
		yield return new WaitForSeconds (1.6f);
		GameManagerLevel3.instance.toChangeAlphaValue.transform.DOScale (Vector3.zero, 0f);

		GameManagerLevel3.instance.lab.transform.DOScale (Vector3.zero, 0f);
		GameManagerLevel3.instance.cameraInInstructions [12].SetActive (true);

		GameManagerLevel3.instance.allSkyBox [3].SetActive (true);
		yield return new WaitForSeconds (4f);
		GameManagerLevel3.instance.cameraInInstructions [12].SetActive (false);

		GameManagerLevel3.instance.cameraInInstructions [3].SetActive (true);
		GameManagerLevel3.instance.btnBack.SetActive (true);
		//GameManagerLevel3.instance.btnBack.transform.parent = dpn.DpnCameraRig._instance._center_eye.transform.parent.parent;
		GameManagerLevel3.instance.medicalGas.enabled = true;

	}

	public void OnClickWellDoneMedicalGas ()
	{
		SoundManager.instance.PlayClickSound ();
		//TrollyControllerwithAngle.instance.isFront = true;

		GameManagerLevel3.instance.isMedicalGasActivity = false;
		GameManagerLevel3.instance.isWaterActivity = true;
		GameManagerLevel3.instance.afterPlacedmsg [3].SetActive (false);
		GameManagerLevel3.instance.cameraInInstructions [0].SetActive (true);
		GameManagerLevel3.instance.activityArea [3].SetActive (false);
		GameManagerLevel3.instance.baseRotateMedical.transform.GetComponent<ToRotateBase> ().enabled = false;
		GameManagerLevel3.instance.raysMedical.SetActive (false);
	}

	#endregion

	#region Activity5

	//	<summary> this region is for the actvity5(watergasActivity)
	//	</summary>
	public void OnClickWaterGas ()
	{
		SoundManager.instance.PlayClickSound ();
		DisableLables ();
		GameManagerLevel3.instance.cameraInInstructions [14].SetActive (false);
		GameManagerLevel3.instance.toChangeAlphaValue.transform.DOScale (Vector3.one, 0f);

		GameManagerLevel3.instance.toChangeAlphaValue.DOColor (new Color32 (255, 255, 255, 255), 1.5f);
		SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [20]);

		StartCoroutine (EnableWaterGasSkybox ());
		GameManagerLevel3.instance.waterGas.enabled = false;

	}

	IEnumerator EnableWaterGasSkybox ()
	{
		yield return new WaitForSeconds (1.6f);
		GameManagerLevel3.instance.toChangeAlphaValue.transform.DOScale (Vector3.zero, 0f);
	
		GameManagerLevel3.instance.lab.transform.DOScale (Vector3.zero, 0f);
		GameManagerLevel3.instance.cameraInInstructions [15].SetActive (true);
		GameManagerLevel3.instance.allSkyBox [4].SetActive (true);
		yield return new WaitForSeconds (4f);
		GameManagerLevel3.instance.cameraInInstructions [15].SetActive (false);

		GameManagerLevel3.instance.cameraInInstructions [3].SetActive (true);
		GameManagerLevel3.instance.btnBack.SetActive (true);
		//GameManagerLevel3.instance.btnBack.transform.parent = dpn.DpnCameraRig._instance._center_eye.transform.parent.parent;
		GameManagerLevel3.instance.waterGas.enabled = true;

	}

	public void OnClickWellDoneWaterGas ()
	{
		SoundManager.instance.PlayClickSound ();
		//TrollyControllerwithAngle.instance.isFront = true;
	
		GameManagerLevel3.instance.afterPlacedmsg [4].SetActive (false);
		GameManagerLevel3.instance.cameraInInstructions [0].SetActive (true);
		GameManagerLevel3.instance.activityArea [4].SetActive (false);
		GameManagerLevel3.instance.baseRotateWater.transform.GetComponent<ToRotateBase> ().enabled = false;
		GameManagerLevel3.instance.raysWater.SetActive (false);
		GameManagerLevel3.instance.CheckScore ();
	}

	#endregion

	void EnableLables ()
	{
		for (int i = 0; i < GameManagerLevel3.instance.labels.Length; i++) {
			GameManagerLevel3.instance.labels [i].SetActive (true);
		}

		GameManagerLevel3.instance.scoreLable.SetActive (true);
	}

	void DisableLables ()
	{
		for (int i = 0; i < GameManagerLevel3.instance.labels.Length; i++) {
			GameManagerLevel3.instance.labels [i].SetActive (false);

		}
		GameManagerLevel3.instance.scoreLable.SetActive (false);

	}
}
