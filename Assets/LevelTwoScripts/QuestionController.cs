using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UmetyDatabase;

[System.Serializable]
public  class QuestionData
{
	public string QuestnName;
	public string QuestionID;
	public GameObject QuestionGameObject;
	public List<string> QuestionOptions;
	public GameObject CorrectOption;
	public GameObject WrongOption;

}

public class QuestionController : MonoBehaviour
{
	public static QuestionController Instance;

	public List<QuestionData> questionData;

	public int CurrentQuestionCounter = 0;

	public GameObject score_text;

	public GameObject wellDone, tryAgain, wellDone1, wellDone2, tryAgain1, tryAgain2;

	int wrongOptionClickCounter = 0;
	bool optionClicked = false;

	// Use this for initialization
	void Start ()
	{
		Instance = this;
	}

	void Update ()
	{
	}

	void scoreCount ()
	{
		if (wrongOptionClickCounter == 0) {
			PlayerPrefs.SetInt ("score", PlayerPrefs.GetInt ("score") + 5);
		}

		if (wrongOptionClickCounter < 1) {

//			if (GameManagerLevelThree.Instance.counter == 1) {
//				DatabaseManager.dbm.AddEntry ("L3", "Q1", wrongOptionClickCounter + 1, curr_Score, true, false);
//			} 
//			if (GameManagerLevelThree.Instance.counter == 2) {
//				DatabaseManager.dbm.AddEntry ("L3", "Q2", wrongOptionClickCounter + 1, curr_Score, true, false);
//			} 
//			if (GameManagerLevelThree.Instance.counter == 3) {
//				DatabaseManager.dbm.AddEntry ("L3", "Q3", wrongOptionClickCounter + 1, curr_Score, true, false);
//			} 
//			if (GameManagerLevelThree.Instance.counter == 4) {
//				DatabaseManager.dbm.AddEntry ("L3", "Q4", wrongOptionClickCounter + 1, curr_Score, true, false);
//			} 
//			if (GameManagerLevelThree.Instance.counter == 5) {
//				DatabaseManager.dbm.AddEntry ("L3", "Q5", wrongOptionClickCounter + 1, curr_Score, true, false);
//			} 
		} 


	}

	public void RightOption ()
	{
		if (!optionClicked) {
			optionClicked = true;

			if (wrongOptionClickCounter > 0) {
				questionData [CurrentQuestionCounter].CorrectOption.GetComponentInChildren<Text> ().color = new Color32 (105, 223, 0, 255);
				questionData [CurrentQuestionCounter].CorrectOption.GetComponentInChildren<Image> ().raycastTarget = false;
				questionData [CurrentQuestionCounter].WrongOption.GetComponentInChildren<Image> ().raycastTarget = false;
				StartCoroutine (RightOption_waitTime ());

				StartCoroutine ("OkHandle", 2f);
			} else {
				//SoundManager.instance.PlayRightSound ();
				questionData [CurrentQuestionCounter].CorrectOption.GetComponentInChildren<Text> ().color = new Color32 (105, 223, 0, 255);
				questionData [CurrentQuestionCounter].CorrectOption.GetComponentInChildren<Image> ().raycastTarget = false;
				questionData [CurrentQuestionCounter].WrongOption.GetComponentInChildren<Image> ().raycastTarget = false;
//				GameManagerLevelThree.Instance.counter++;
				if (CurrentQuestionCounter == 0)
					wellDone.SetActive (true);
				if (CurrentQuestionCounter == 1)
					wellDone1.SetActive (true);
				if (CurrentQuestionCounter == 2)
					wellDone2.SetActive (true);
				StartCoroutine (RightOption_waitTime ());
				StartCoroutine ("OkHandle", 2f);
			}
			scoreCount ();
		}
	}

	IEnumerator OkHandle (float delay)
	{
		yield return new WaitForSeconds (delay);
		GameManagerTwo.instance.questionsOk [CurrentQuestionCounter - 1].SetActive (true);
		wrongOptionClickCounter = 0;
	}


	public void wrongOption (GameObject Option)
	{
		if (!optionClicked) {
			Option.GetComponent <Image> ().raycastTarget = false;
			optionClicked = true;
			wrongOptionClickCounter++;
		
			Option.GetComponentInChildren<Text> ().color = new Color32 (255, 0, 32, 255); 
			tryAgain.SetActive (true);
			tryAgain1.SetActive (true);
			tryAgain2.SetActive (true);
			Invoke ("CorrectResponse", 2f);

			questionData [CurrentQuestionCounter].CorrectOption.GetComponentInChildren<Image> ().raycastTarget = false;
			questionData [CurrentQuestionCounter].WrongOption.GetComponentInChildren<Image> ().raycastTarget = false;
			StartCoroutine (wrongOption_waitTime (Option));
		}
	}

	IEnumerator wrongOption_waitTime (GameObject Option)
	{
		yield return new WaitForSeconds (1f);
		tryAgain.SetActive (false);
		tryAgain1.SetActive (false);
		tryAgain2.SetActive (false);
		optionClicked = false;
		if (wrongOptionClickCounter > 0) {
			RightOption ();
		}
	}

	IEnumerator RightOption_waitTime ()
	{
		yield return new WaitForSeconds (1f);

		CurrentQuestionCounter++;
		wellDone.SetActive (false);
		wellDone1.SetActive (false);
		wellDone2.SetActive (false);      
		optionClicked = false;

	}

	void CorrectResponse ()
	{
		LanguageHandler.instance.PlayVoiceOver ("C_Correct_Response");

		if (wrongOptionClickCounter > 0) {
//			if (GameManagerLevelThree.Instance.counter == 1) {
//				DatabaseManager.dbm.AddEntry ("L3", "Q1", 1, 0, false, false);
//			}
//			if (GameManagerLevelThree.Instance.counter == 2) {
//				DatabaseManager.dbm.AddEntry ("L3", "Q2", 1, 0, false, false);
//			}
//			if (GameManagerLevelThree.Instance.counter == 3) {
//				DatabaseManager.dbm.AddEntry ("L3", "Q3", 1, 0, false, false);
//			}
//			if (GameManagerLevelThree.Instance.counter == 4) {
//				DatabaseManager.dbm.AddEntry ("L3", "Q4", 1, 0, false, false);
//			}
//			if (GameManagerLevelThree.Instance.counter == 5) {
//				DatabaseManager.dbm.AddEntry ("L3", "Q5", 1, 0, false, false);
//			}
		}
		wrongOptionClickCounter = -1;
	}


}
