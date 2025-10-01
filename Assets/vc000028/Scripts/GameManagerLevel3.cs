using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UmetyDatabase;

public class GameManagerLevel3 : MonoBehaviour
{
	public static GameManagerLevel3 instance;

	//	<summary>  Here all the variables are defined
	//	</summary>

	[Header ("Instructions And Labels")]
	public GameObject[] cameraInInstructions;
	public GameObject[] instructions;
	public GameObject[] labels;
	public GameObject rays, baseRotate;
	public GameObject[] activityArea;
	public Image toChangeAlphaValue;
	public GameObject[] allSkyBox;
	public GameObject lab;
	public GameObject btnBack;
	public GameObject target1, target2;
	public GameObject copperMove;
	public GameObject tryAgain, wellDone;
	public GameObject raysSilver, baseRotateSilver;
	public GameObject raysMethane, baseRotateMethane;
	public GameObject raysMedical, baseRotateMedical;
	public GameObject raysWater, baseRotateWater;


	public GameObject[] afterPlacedmsg;

	public EventTrigger copperBar, silverBar, cylinderMethane, medicalGas, waterGas;

	[Header ("Animation And Clips")]
	public Animation activityAnima;
	public AnimationClip[] clip;

	internal bool isCopperActivity, isSilverActivity, isMethaneActivity, isMedicalGasActivity, isWaterActivity;
	internal bool isPlacedCopper, isPlacedSilver, isPlacedCylinder, isPlacedMedical, isPlacedWater;

	public Text percentText;
	public GameObject wellDoneComplete, greaterThan60, lessThan60;
	public GameObject gvrRectical;
	public Transform vrPlayer;
	public GameObject trooly;
	public GameObject scoreLable;

	internal bool isTriggered;
	internal int dragCount;
	public GameObject greenMixture, greenPuresubstance;

	// Use this for initialization
	void Start ()
	{
		instance = this;
		dragCount = 0;

	}


	public void AnimaitonPlay (AnimationClip clip)
	{

		activityAnima.clip = clip;
		activityAnima.Play ();

	}

	#region Afterdropping

	//	<summary>  After Dropping the objects to it's correct place these functions executes respectively
	//	</summary>
	public IEnumerator DragCopper (RaycastHit _hit, GameObject g)
	{
		cameraInInstructions [4].SetActive (false);

		Invoke ("DisablewellDone", 2f);
		cameraInInstructions [4].SetActive (false);
		g.transform.DOMove (target1.transform.position, 1f);
		g.transform.DOScale (new Vector3 (.4f, .4f, .4f), .2f);
		g.transform.SetParent (_hit.transform);
		yield return new WaitForSeconds (2.2f);
		afterPlacedmsg [0].SetActive (true);
	}

	public IEnumerator DragSilver (RaycastHit _hit, GameObject g)
	{
		Invoke ("DisablewellDone", 2f);

		cameraInInstructions [7].SetActive (false);

		g.transform.DOMove (target2.transform.position, 1f);
		g.transform.DOScale (new Vector3 (.4f, .4f, .4f), .2f);
		g.transform.SetParent (_hit.transform);
		yield return new WaitForSeconds (2.2f);
		afterPlacedmsg [1].SetActive (true);

	}

	public IEnumerator DragCylinder (RaycastHit _hit, GameObject g)
	{
		Invoke ("DisablewellDone", 2f);
	
		cameraInInstructions [10].SetActive (false);

		g.transform.DOMove (target1.transform.position, 1f);
		g.transform.DOScale (new Vector3 (.4f, .4f, .4f), .2f);
		g.transform.SetParent (_hit.transform);
		yield return new WaitForSeconds (2.2f);
		afterPlacedmsg [2].SetActive (true);
	}

	public IEnumerator DragMedicalGas (RaycastHit _hit, GameObject g)
	{
		Invoke ("DisablewellDone", 2f);

		cameraInInstructions [13].SetActive (false);

		g.transform.DOMove (target2.transform.position, 1f);
		g.transform.DOScale (new Vector3 (.4f, .4f, .4f), .2f);
		g.transform.SetParent (_hit.transform);
		yield return new WaitForSeconds (2.2f);
		afterPlacedmsg [3].SetActive (true);

	}

	public IEnumerator DragSalineWater (RaycastHit _hit, GameObject g)
	{
		Invoke ("DisablewellDone", 2f);

		cameraInInstructions [16].SetActive (false);
		//MovementController.Instance.enabled = false;
		g.transform.DOMove (target2.transform.position, 1f);
		g.transform.DOScale (new Vector3 (.4f, .4f, .4f), .2f);
		g.transform.SetParent (_hit.transform);
		yield return new WaitForSeconds (2.2f);
		afterPlacedmsg [4].SetActive (true);


	}

	void DisablewellDone ()
	{
		wellDone.SetActive (false);
	
	}

	#endregion

	#region scoremanger


	//	<summary>  calculating the user score and checking either less than60 or greater than60
	//	</summary>
	float per;

	float CalculatePercentage ()
	{
		return (PlayerPrefs.GetInt ("C_Scrore") * 100) / 5;
	}

	public void CheckScore ()
	{
		
		per = CalculatePercentage ();

		if (LanguageHandler.instance.IsRightToLeft) {

			percentText.text = "" + "%" + per;

		} else
			percentText.text = "" + per + "%";

		wellDoneComplete.SetActive (true);

	}

	public void ClickOnFinalScorePanel ()
	{
		SoundManager.instance.PlayClickSound ();
		//DatabaseManager.dbm.InsertFinalDataToDatabase ();
		wellDoneComplete.SetActive (false);

		if (per >= 60) {

			greaterThan60.SetActive (true); 

		} else {

			lessThan60.SetActive (true); 
		}

	}

	public void ClickOnWellDoneLOCanvas ()
	{
		SoundManager.instance.PlayClickSound ();
		greaterThan60.SetActive (false); 
		lessThan60.SetActive (false); 

		LoadingScene.LoadingSceneIndex = 1;

	}

	#endregion

}
