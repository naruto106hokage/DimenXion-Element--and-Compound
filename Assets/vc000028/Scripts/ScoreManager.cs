using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SmartLocalization;
using ArabicSupport;
using UmetyDatabase;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager instance;
	public Text scoreText;

	public int WrongCount;
	// Use this for initialization
	void Start ()
	{
		PlayerPrefs.SetInt ("C_Scrore", 0);
		instance = this;
	}

	// Update is called once per frame
	void Update ()
	{
		

		if (!LanguageHandler.instance.IsRightToLeft) {
			if (PlayerPrefs.GetString("currentLanguage") == "mr-IN"|| PlayerPrefs.GetString("currentLanguage") == "hi-IN")
            {
			scoreText.text = "" + LanguageManager.Instance.GetTextValue ("C_Scrore") + "% " + PlayerPrefs.GetInt ("C_Scrore");
			}else{
			scoreText.text = "" + LanguageManager.Instance.GetTextValue ("C_Scrore") + " : " + PlayerPrefs.GetInt ("C_Scrore");
			}
		} else if (LanguageHandler.instance.IsRightToLeft) {
			scoreText.text = PlayerPrefs.GetInt ("C_Scrore") + " : " + ArabicFixer.Fix ("" + LanguageManager.Instance.GetTextValue ("C_Scrore"), false, false);
		}
	}

	/// <summary>
	/// entering the score of user into the database of successful attempt.
	/// </summary>
	public void ScoreCount (string questionID)
	{
		if (WrongCount == 0) {
			PlayerPrefs.SetInt ("C_Scrore", PlayerPrefs.GetInt ("C_Scrore") + 1);
			SetEntry (1, true, questionID);
		} else {
			PlayerPrefs.SetInt ("C_Scrore", PlayerPrefs.GetInt ("C_Scrore") + 0);
			SetEntry (0, true, questionID);
		}
			
		WrongCount = 0;
	}

	void SetEntry (int us, bool isAutoCorrect, string qID)
	{
		DatabaseManager.dbm.AddEntry ("AS", qID, 1, us, isAutoCorrect, false, false);
	}

}


