using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LitJson;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class VoiceAssistanceHandler : MonoBehaviour
{
	public static VoiceAssistanceHandler instance;


	internal string ClassPath = "broadcastplugin.umety.com.masterappbroadcast.DActivity";

	AndroidJavaObject Context;

	AndroidJavaObject ClassName;

	int currentVolume;
	int maxVolume;

	public GameObject volumeControl;
	public RectTransform volumeBarFilledRect;

	ChatbotManager chatBotManager;
	VoiceAssistanceStaticData voiceStaticData;
	ApiAiCustomResponseManager apiAiCustomResponseManager;
	VoiceAssistanceManager voiceAssistanceManager;

	AlexaManager alexaManager;
	//	BackMenu bm;


	SpeechRecognizationManager speechrecognizermanager;

	void Awake ()
	{
		instance = this;
	}

	void Start ()
	{
		voiceStaticData = GetComponent<VoiceAssistanceStaticData> ();
		apiAiCustomResponseManager = GetComponent<ApiAiCustomResponseManager> ();
		voiceAssistanceManager = GetComponent<VoiceAssistanceManager> ();
		chatBotManager = GetComponent<ChatbotManager> ();
		alexaManager = GetComponent<AlexaManager> ();
		speechrecognizermanager = GetComponent<SpeechRecognizationManager> ();
		//		bm = GetComponent<BackMenu>();

	}

	internal void SetAction ()
	{
		//----- SETTING TYPE OF ACTION ----- 

		voiceAssistanceManager.currentActionParams.Clear ();

		switch (apiAiCustomResponseManager.currentResponse.Result.Action) {
		case "module.goto.level":
			voiceAssistanceManager.currentVoiceAction = TypeOfVoiceActions.navigateScene;
			break;

		case "module.close":
			voiceAssistanceManager.currentVoiceAction = TypeOfVoiceActions.exitApp;
			break;

		case "module.setting.volume":
			voiceAssistanceManager.currentVoiceAction = TypeOfVoiceActions.settingSelection;
			break;

		case "module.goto.screen":
			voiceAssistanceManager.currentVoiceAction = TypeOfVoiceActions.navigateActions;
			break;

		case "module.action.restart":
			voiceAssistanceManager.currentVoiceAction = TypeOfVoiceActions.restart;
			break;

		case "module.close.no":
			voiceAssistanceManager.currentVoiceAction = TypeOfVoiceActions.confirmation_no;
			break;

		case "module.close.yes":
			voiceAssistanceManager.currentVoiceAction = TypeOfVoiceActions.confirmation_yes;
			break;

		case "module.action.back":
			voiceAssistanceManager.currentVoiceAction = TypeOfVoiceActions.navigateBackMenu;
			break;

		default :
			voiceAssistanceManager.currentVoiceAction = TypeOfVoiceActions.other;
			break;
		}

		//----- SETTING PARAMETER OF ACTION ----- 

		List<string> keyList = apiAiCustomResponseManager.currentResponse.Result.Parameters.Keys.ToList ();

		for (int i = 0; i < apiAiCustomResponseManager.currentResponse.Result.Parameters.Count; i++) {
			Parameter p = new Parameter ();
			p.parameterName = keyList [i];
			p.parameterValue = apiAiCustomResponseManager.currentResponse.Result.Parameters [p.parameterName].ToString ();

			voiceAssistanceManager.currentActionParams.Add (p);
		}
	}

	/// <Perform actions when voice action matched>
	/// Dos the action.
	/// </summary>
	internal void DoAction ()
	{
		if (voiceAssistanceManager.currentVoiceAction == TypeOfVoiceActions.navigateScene) {
			NavigateScenes ();
		} else if (voiceAssistanceManager.currentVoiceAction == TypeOfVoiceActions.settingSelection) {
			selectionSwitcher ();
		} else if (voiceAssistanceManager.currentVoiceAction == TypeOfVoiceActions.navigateActions) {
			NavigateActions ();
		} else if (voiceAssistanceManager.currentVoiceAction == TypeOfVoiceActions.restart) {
			Restart ();
		} else if (voiceAssistanceManager.currentVoiceAction == TypeOfVoiceActions.confirmation_yes) {
			invokeActionsOn_Yes ();
		} else if (voiceAssistanceManager.currentVoiceAction == TypeOfVoiceActions.navigateBackMenu) {
			enablebackMenu ();
		}
	}

	//-----------------------------------------------------Methods to perform actions--------------------------------//



	/// <Scene_Switcher>
	/// Navigates the scenes.
	/// </summary>
	/// 
	void NavigateScenes ()
	{
		if (apiAiCustomResponseManager.currentResponse.Result.Fulfillment.Data.LevelID != null) {
			if (SceneManager.GetActiveScene ().name == "MainMenu") {
				string levelName = apiAiCustomResponseManager.currentResponse.Result.Fulfillment.Data.LevelID.ToString ();
				#region OnlyOneSceneinTwoLevels
				//if you are using L1(Scene) for two levels L1 & L2
				// Open level 1 according to main menu if you get levelName as L1
				// Open level 2 according to main menu if you get levelName as L2
				//and modified line SceneManager.LoadScene(levelName) accordingly
				#endregion
				SceneManager.LoadScene (levelName);
				SoundManager.instance.PlayClickSound ();
			}
		}
	}

	public const string SceneNotAvailableCommon = "Sorry, you can access other level from Main menu scene only.";

	int screenParamIndex;

	#region NavigateFromLevelToMainMenu

	internal void IsNavigateScenesExists ()
	{
		if (apiAiCustomResponseManager.currentResponse.Result.Fulfillment.Data.LevelID != null) {
			if (SceneManager.GetActiveScene ().name != "MainMenu") {
				SpeechRecognizationManager.Instance.aiResponse.Result.Fulfillment.Speech = SceneNotAvailableCommon;
			}
		} else {
			if (SceneManager.GetActiveScene ().name == "MainMenu") {
				SpeechRecognizationManager.Instance.aiResponse.Result.Fulfillment.Speech = "Sorry, this module doesn't consist this level";
			}
		}
	}

	#endregion

	public bool isclickable;

	/// <navigate instructional actions>
	/// Navigates the actions.
	/// </summary>
	void NavigateActions ()
	{
		int screenParamIndex = 0;

		for (int i = 0; i < voiceAssistanceManager.currentActionParams.Count; i++) {
			if (voiceAssistanceManager.currentActionParams [i].parameterName == "screen") {
				screenParamIndex = i;
			}

			switch (voiceAssistanceManager.currentActionParams [screenParamIndex].parameterValue) {
			case "MainMenu":
				if (SceneManager.GetActiveScene ().name != "MainMenu") {
					SceneManager.LoadScene ("MainMenu");
					SoundManager.instance.PlayClickSound ();
				}			
				break;

			case "LO":
				if (SceneManager.GetActiveScene ().name == "MainMenu") {
					SceneManager.LoadScene ("LO");
					SoundManager.instance.PlayClickSound ();
				}
				break;

			case "AS":
                    #region IfYouMergeAnyLevelWithAssessYourKnowledge
                    //if you are using AS(Scene) for two levels L1 & AS
                    // Open AS according to main menu if you get levelName as AS
                    // Open level1 according to main menu in NavigateScenes() method
                    //with  SceneManager.LoadScene("AS") along with your Condition
                    //and modified line SceneManager.LoadScene(levelName) accordingly
                    #endregion
				if (SceneManager.GetActiveScene ().name == "MainMenu") {
					SceneManager.LoadScene ("AS");
					SoundManager.instance.PlayClickSound ();
				}
				break;
			}
		}
	}

	public const string NavigateActionsStr = "Sorry, you are already in main menu scene";

	internal void IsNavigateActions ()
	{
		int screenParamIndex = 0;

		for (int i = 0; i < voiceAssistanceManager.currentActionParams.Count; i++) {
			if (voiceAssistanceManager.currentActionParams [i].parameterName == "screen") {
				screenParamIndex = i;
			}

			switch (voiceAssistanceManager.currentActionParams [screenParamIndex].parameterValue) {

			case "MainMenu":
				if (SceneManager.GetActiveScene ().name == "MainMenu") {
					SpeechRecognizationManager.Instance.aiResponse.Result.Fulfillment.Speech = NavigateActionsStr;
				}
				break;
			case "LO":
				if (SceneManager.GetActiveScene ().name != "MainMenu") {
					SpeechRecognizationManager.Instance.aiResponse.Result.Fulfillment.Speech = SceneNotAvailableCommon;
				}
				break;
			case "AS":
				if (SceneManager.GetActiveScene ().name != "MainMenu") {
					SpeechRecognizationManager.Instance.aiResponse.Result.Fulfillment.Speech = SceneNotAvailableCommon;
				}
				break;
			}
		}
	}

	void invokeActionsOn_Yes ()
	{
		string s_mode = PlayerPrefs.GetString ("QuitMode");

		if (s_mode == "Quittable") {
			System.Diagnostics.Process.GetCurrentProcess ().Kill ();
         
			Application.Quit ();
		}
	}

	public const string isQuittableStr = "Sorry, quit is not available in this mode";

	internal void isQuitable ()
	{
		string s_mode = PlayerPrefs.GetString ("QuitMode");

		if (s_mode == "NonQuittable") {
			SpeechRecognizationManager.Instance.aiResponse.Result.Fulfillment.Speech = isQuittableStr;
		} 
	}

	void Restart ()
	{
		Time.timeScale = 1;
		SoundManager.instance.PlayClickSound ();
		LoadingScene.LoadingSceneIndex = SceneManager.GetActiveScene ().buildIndex;
	}

	void enablebackMenu ()
	{
		string s_mode = PlayerPrefs.GetString ("QuitMode");

		if (s_mode == "NonQuittable")
			return;

		if (SceneManager.GetActiveScene ().buildIndex != 1) {			
			BackMenu.instance.ToggleBackMenu ();
		}
	}

	public const string backMenuStr = "Sorry, back menu is not available in main menu scene";
	public const string isBackableStr = "Sorry, back menu is not available in this mode";

	internal void isBackMenuEnabled ()
	{
		string s_mode = PlayerPrefs.GetString ("QuitMode");

		if (s_mode == "NonQuittable") {
			SpeechRecognizationManager.Instance.aiResponse.Result.Fulfillment.Speech = isBackableStr;
			return;
		}

		if (SceneManager.GetActiveScene ().buildIndex == 1) {
			SpeechRecognizationManager.Instance.aiResponse.Result.Fulfillment.Speech = backMenuStr;
		} 
	}

	void selectionSwitcher ()
	{
		int screenParamIndex = 0;

		for (int i = 0; i < voiceAssistanceManager.currentActionParams.Count; i++) {
			if (voiceAssistanceManager.currentActionParams [i].parameterName == "change")
				screenParamIndex = i;
		}

		switch (voiceAssistanceManager.currentActionParams [screenParamIndex].parameterValue) {

		case "increase":
			ChangeVolume ();
			break;
		case "decrease":
			ChangeVolume ();
			break;
		}	
	}

	/// <volume controls>
	/// Changes the volume.
	/// </summary>
	void ChangeVolume ()
	{

		volumeControl.SetActive (true);
		#if UNITY_ANDROID && !UNITY_EDITOR

		ClassName = alexaManager.ClassName;
		Context = alexaManager.Context;

		currentVolume = ClassName.Call<int> ("getCurrentVolume", Context);
		maxVolume = ClassName.Call<int> ("getMaxVolume", Context);

		float currentWidth = (((float)currentVolume) / maxVolume) * voiceStaticData.volumeBarTotalWidth;
		float finalWidth;

		volumeBarFilledRect.sizeDelta = new Vector2 (currentWidth, volumeBarFilledRect.sizeDelta.y);

		if (apiAiCustomResponseManager.currentResponse.Result.Parameters ["change"].ToString () == "increase" ) {

		currentVolume += voiceStaticData.volumeChangeAmount;

		if (currentVolume > maxVolume)
		currentVolume = maxVolume;

		finalWidth = (((float)currentVolume) / maxVolume) * voiceStaticData.volumeBarTotalWidth;
		StartCoroutine (VolumeProgress (currentWidth, finalWidth));

		} else if (apiAiCustomResponseManager.currentResponse.Result.Parameters ["change"].ToString () == "decrease") {
		currentVolume -= voiceStaticData.volumeChangeAmount;

		if (currentVolume < 0)
		currentVolume = 0;

		finalWidth = (((float)currentVolume) / maxVolume) * voiceStaticData.volumeBarTotalWidth;
		StartCoroutine (VolumeProgress (currentWidth, finalWidth));

		}
		else{
		chatBotManager.ChatbotGameobject.transform.DOScale (0, voiceStaticData.chatBotPanelRate_Android_OUT);
		volumeControl.SetActive (false);
		}
		ClassName.Call ("changeVolume", currentVolume, Context);
		#endif
	}

	IEnumerator VolumeProgress (float start, float end)
	{
		float current = start;

		if (start < end) {

			while (current <= end) {
				volumeBarFilledRect.sizeDelta = new Vector2 (current, volumeBarFilledRect.sizeDelta.y);
				current += 0.025f;
				yield return null;
			}

		} else {

			while (current >= end) {
				volumeBarFilledRect.sizeDelta = new Vector2 (current, volumeBarFilledRect.sizeDelta.y);
				current -= 0.025f;
				yield return null;
			}
		}

		yield return new WaitForSeconds (0.5f);
		// In volume case doscaling stopped until filling of volume bar
		chatBotManager.ChatbotGameobject.transform.DOScale (0, voiceStaticData.chatBotPanelRate_Android_OUT);
		volumeControl.SetActive (false);
	}

}