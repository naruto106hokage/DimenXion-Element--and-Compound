using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AllCouroutineL2 : MonoBehaviour
{
	public static AllCouroutineL2 instance;

	void Start ()
	{
		instance = this;
	}



	IEnumerator AfterMetalPlacedOnTable ()
	{
		float len = GameManagerTwo.instance.tableOneClips [0].length;
		yield return new WaitForSeconds (len);
		GameManagerTwo.instance.anim.Play ("rotatePlate");
		len = LanguageHandler.instance.GetLength ("L2_Inst_TakeaLook").length;
		yield return new WaitForSeconds (len);
		GameManagerTwo.instance.cameraInInstructions [11].SetActive (false);

		GameManagerTwo.instance.q1.SetActive (true);

	}


	IEnumerator AfterMetalPlacedOnTableOne ()
	{
		float len = GameManagerTwo.instance.tableOneClips [7].length;
		yield return new WaitForSeconds (len);
		GameManagerTwo.instance.instructions [1].SetActive (true);
		GameManagerTwo.instance.details [0].SetActive (true);
		GameManagerTwo.instance.details [1].SetActive (true);
		GameManagerTwo.instance.labels [0].SetActive (true);
		GameManagerTwo.instance.labels [1].SetActive (true);
		GameManagerTwo.instance.labels [2].SetActive (true);


	}


	public	IEnumerator AfterIronMetal ()
	{
		float len = GameManagerTwo.instance.tableOneClips [1].length;
		yield return new WaitForSeconds (len);
		GameManagerTwo.instance.details [7].SetActive (true);
		GameManagerTwo.instance.details [2].SetActive (true);
		GameManagerTwo.instance.instructions [2].SetActive (true);
		GameManagerTwo.instance.labels [0].SetActive (true);
		GameManagerTwo.instance.labels [1].SetActive (true);
		GameManagerTwo.instance.labels [2].SetActive (true);



	}

	public	IEnumerator AfterIronMixtureMetal ()
	{
		float len = GameManagerTwo.instance.tableOneClips [2].length;
		yield return new WaitForSeconds (len);
		GameManagerTwo.instance.details [3].SetActive (true);
		GameManagerTwo.instance.cameraInInstructions [4].SetActive (false);
		GameManagerTwo.instance.q2.SetActive (true);



	}

	public	IEnumerator AfterClickBottleCap ()
	{
		float len = GameManagerTwo.instance.tableOneClips [3].length;
		yield return new WaitForSeconds (len);
		GameManagerTwo.instance.cameraInInstructions [7].SetActive (true);
		GameManagerTwo.instance.solutionBottleCollider.GetComponent<BoxCollider> ().enabled = true;
		GameManagerTwo.instance.cylinderCollider.SetActive (true);
	}

	public	IEnumerator AfterPourSolutionIntoCylinder ()
	{
		float len = GameManagerTwo.instance.tableOneClips [4].length;
		yield return new WaitForSeconds (len);
		GameManagerTwo.instance.labels [3].SetActive (true);
		GameManagerTwo.instance.cameraInInstructions [8].SetActive (true);
		GameManagerTwo.instance.graduateCylinderColllider.SetActive (true);
		GameManagerTwo.instance.graduateCylinder.SetActive (false);

	}

	public	IEnumerator AfterPourSolutionIntoBeakerA ()
	{
		float len = GameManagerTwo.instance.tableOneClips [5].length;
		yield return new WaitForSeconds (len);
		GameManagerTwo.instance.labels [4].SetActive (true);
		GameManagerTwo.instance.details [8].SetActive (true);
		GameManagerTwo.instance.details [4].SetActive (true);
		GameManagerTwo.instance.graduateCylinderHalfFillColllider.SetActive (true);
		GameManagerTwo.instance.graduateCylinder.SetActive (false);
		GameManagerTwo.instance.beakerACollider.SetActive (false);
		GameManagerTwo.instance.instructions [4].SetActive (true);
	}

	public	IEnumerator AfterPourSolutionIntoBeakerB ()
	{
		float len = GameManagerTwo.instance.tableOneClips [6].length;
		yield return new WaitForSeconds (len);
		GameManagerTwo.instance.labels [4].SetActive (true);
		GameManagerTwo.instance.details [5].SetActive (true);
		GameManagerTwo.instance.cameraInInstructions [10].SetActive (false);
		GameManagerTwo.instance.instructions [5].SetActive (true);

	}
}
