using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKSpeech;
using UnityEngine.UI;
using InfinityEngine.Localization;
using InfinityEngine.Extensions;
using System.Linq;
using System.IO;
using System.Reflection;
using ApiAiSDK;
using ApiAiSDK.Model;
using ApiAiSDK.Unity;
using Newtonsoft.Json;
using System.Net;
using System;
using LitJson;

public class SpeechAndTextManager : MonoBehaviour
{
	public static SpeechAndTextManager instance;

	public enum SPEECHAISTATE
	{
		ENABLE,
		DISABLE

	}

	public SPEECHAISTATE mySpeechAiState;
	public Text SpeechToTextResultText;

	void Awake ()
	{
		instance = this;
	}

	void Start ()
	{
		InitTextToSpeech ();
		StartCoroutine (InitSpeechToText ());

//		For testing
//		TextAsset DataPath= Resources.Load(localUrlSpeechDemoJson) as TextAsset;
//		string DataPathText= DataPath.ToString();
//		ParseResultText (DataPathText);

	}

	void Update ()
	{
		if (apiAiUnity != null) {
			apiAiUnity.Update ();
		}

		// dispatch stuff on main thread
		while (ExecuteOnMainThread.Count > 0) {
			ExecuteOnMainThread.Dequeue ().Invoke ();
		}

		if (!TextToSpeechIntialized)
			return;

		if (SpeechEngine.IsEnabled && SpeechEngine.IsReady) {
			informations.text = "Speech Engine is ready to speak in " + SpeechEngine.CurrentLocale.Name;
		} else {
			informations.text = string.Format ("Speech Engine is not ready to speak\n\nError Type : {0}", SpeechEngine.Status);
		}

	}

	#region TextToSpeech

	public CanvasGroup settingPane;

	public Slider pitchSlider;
	public Slider speechRateSlider;

	public InputField input;

	public Dropdown localesDropdown;
	public Dropdown voicesDropdown;
	public Dropdown enginesDropdown;

	private TTSEngine[] engines;
	private Locale[] locales;
	private Voice[] voices;

	public Text informations;

	bool TextToSpeechIntialized = false;

	void InitTextToSpeech ()
	{
		SpeechEngine.AddCallback (() => {
			locales = Locale.AllLocales; // SpeechEngine.AvailableLocales;
			voices = SpeechEngine.AvaillableVoices;
			engines = SpeechEngine.AvailableEngines;

			localesDropdown.AddOptions (locales.Select (elem => elem.Informations).ToList ());
			voicesDropdown.AddOptions (voices.Select (elem => elem.Name).ToList ());
			enginesDropdown.AddOptions (engines.Select (elem => elem.Label).ToList ());

			SpeechEngine.AddListeners (null, pitchSlider, speechRateSlider);

			localesDropdown.onValueChanged.AddListener (value => {
				SpeechEngine.SetLanguage (locales [value]);
				voicesDropdown.ClearOptions ();
				voices = SpeechEngine.AvaillableVoices;
				voicesDropdown.AddOptions (voices.Select (elem => elem.Name).ToList ());

			});

			voicesDropdown.onValueChanged.AddListener (value => {
				SpeechEngine.SetVoice (voices [value]);
			});

			enginesDropdown.onValueChanged.AddListener (value => {
				SpeechEngine.SetEngine (engines [value]);
			});

			TextToSpeechIntialized = true;
			OnTextToSpeechReady ();

		});
	}


	public void OpenSetting ()
	{
		settingPane.DOFadeIn ().Start ();
	}

	public void CloseSetting ()
	{
		settingPane.DOFadeOut ().Start ();
	}

	public void Speak (string s)
	{
		SpeechEngine.Speak (s);
	}

	public void OnTextToSpeechReady ()
	{
		StartCoroutine (DelayPlaySpeechOnStart ());
	}

	IEnumerator DelayPlaySpeechOnStart ()
	{
//		TextToSpeech ("Hi , Welcome to knowledge map");
		yield return new WaitForSeconds (3);
//		TextToSpeech ("Click start to enter map");
	}

	IEnumerator DelayAfterGamePlayStart ()
	{
		TextToSpeech ("You can see all topics around you");
		yield return new WaitForSeconds (3);
		TextToSpeech ("Which topic would you like to learn today");

		yield return new WaitForEndOfFrame ();

	}

	public void TextToSpeech (string query)
	{

		if (mySpeechAiState == SPEECHAISTATE.ENABLE) {
			SpeechEngine.Speak (query);
		}
	}

	public void TextToSpeechWithPause (string query)
	{

		if (mySpeechAiState == SPEECHAISTATE.ENABLE) {
			SpeechEngine.SpeakWithPause (query, 1); 
		}
	}


	#endregion

	#region SpeechToText_UsingAPI.AI

	public Text inputTextField;
	private ApiAiUnity apiAiUnity;
	private AudioSource aud;
	public AudioClip listeningSound;

	private readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings { 
		NullValueHandling = NullValueHandling.Ignore,
	};

	private readonly Queue<Action> ExecuteOnMainThread = new Queue<Action> ();

	// Use this for initialization
	IEnumerator InitSpeechToText ()
	{
		// check access to the Microphone
		yield return Application.RequestUserAuthorization (UserAuthorization.Microphone);
		if (!Application.HasUserAuthorization (UserAuthorization.Microphone)) {
			throw new NotSupportedException ("Microphone using not authorized");
		}

		ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => {
			return true;
		};

		const string ACCESS_TOKEN = "ab50d45781174dbbaddd59349db145f3";

		var config = new AIConfiguration (ACCESS_TOKEN, SupportedLanguage.English);

		apiAiUnity = new ApiAiUnity ();
		apiAiUnity.Initialize (config);

		apiAiUnity.OnError += HandleOnError;
		apiAiUnity.OnResult += HandleOnResult;
	}

	void HandleOnResult (object sender, AIResponseEventArgs e)
	{
		RunInMainThread (() => {
			var aiResponse = e.Response;
			if (aiResponse != null) {
				Debug.Log (aiResponse.Result.ResolvedQuery);
				var outText = JsonConvert.SerializeObject (aiResponse, jsonSettings);

				Debug.Log (outText);

				SpeechToTextResultText.text = outText;

				ParseResultText (outText);
			} else {
				Debug.LogError ("Response is null");
			}
		});
	}


	public string localUrlSpeechDemoJson;

	void ParseResultText (string text)
	{
		JsonData itemData = JsonMapper.ToObject (text);

		print (itemData ["id"].ToString ());
		print (itemData ["result"] ["resolvedQuery"]);
		print (itemData ["result"] ["parameters"] ["Module"]);
		print (itemData ["result"] ["action"]);
		Speak (itemData ["result"] ["fulfillment"] ["speech"].ToString ());
	}

	void HandleOnError (object sender, AIErrorEventArgs e)
	{
		RunInMainThread (() => {
			Debug.LogException (e.Exception);
			Debug.Log (e.ToString ());
			SpeechToTextResultText.text = e.Exception.Message;
		});
	}

	private void RunInMainThread (Action action)
	{
		ExecuteOnMainThread.Enqueue (action);
	}

	public void PluginInit ()
	{

	}

	public void StartListening ()
	{

		if (SpeechToTextResultText != null) {
			SpeechToTextResultText.text = "Listening...";
		}

		aud = GetComponent<AudioSource> ();
		apiAiUnity.StartListening (aud);

	}

	public void StopListening ()
	{
		try {
	
			if (SpeechToTextResultText != null) {
				SpeechToTextResultText.text = "";
			}

			apiAiUnity.StopListening ();
		} catch (Exception ex) {
			Debug.LogException (ex);
		}
	}

	public void SendText ()
	{
		var text = inputTextField.text;
	
		Debug.Log (text);
	
		AIResponse response = apiAiUnity.TextRequest (text);
	
		if (response != null) {
			Debug.Log ("Resolved query: " + response.Result.ResolvedQuery);
			var outText = JsonConvert.SerializeObject (response, jsonSettings);
	
			Debug.Log ("Result: " + outText);
	
			SpeechToTextResultText.text = outText;
		} else {
			Debug.LogError ("Response is null");
		}
	
	}


	public void StartNativeRecognition ()
	{
		try {
			apiAiUnity.StartNativeRecognition ();
		} catch (Exception ex) {
			Debug.LogException (ex);
		}
	}

	#endregion

	#region SpeechToText

	//	void InitSpeechToText ()
	//	{
	//		if (SpeechRecognizer.ExistsOnDevice ()) {
	//			SpeechRecognizerListener listener = GameObject.FindObjectOfType<SpeechRecognizerListener> ();
	//			listener.onAuthorizationStatusFetched.AddListener (OnAuthorizationStatusFetched);
	//			listener.onAvailabilityChanged.AddListener (OnAvailabilityChange);
	//			listener.onErrorDuringRecording.AddListener (OnError);
	//			listener.onErrorOnStartRecording.AddListener (OnError);
	//			listener.onFinalResults.AddListener (OnFinalResult);
	//			listener.onPartialResults.AddListener (OnPartialResult);
	//			listener.onEndOfSpeech.AddListener (OnEndOfSpeech);
	//			//startRecordingButton.enabled = false;
	//			SpeechRecognizer.RequestAccess ();
	//		} else {
	//			SpeechToTextResultText.text = "Sorry, but this device doesn't support speech recognition";
	//			Debug.LogError ("SpeechRecognizer Not Intialized..");
	//			//startRecordingButton.enabled = false;
	//		}
	//	}
	//
	//
	//	public void OnFinalResult (string result)
	//	{
	//		Debug.Log ("UNITY Final Result");
	//		SpeechToTextResultText.text = result;
	//		ActionsOnSpeechToTextResults (result);
	//	}
	//
	//	public void OnPartialResult (string result)
	//	{
	//		Debug.Log ("UNITY Partial Result");
	//		SpeechToTextResultText.text = result;
	//	}
	//
	//	public void OnAvailabilityChange (bool available)
	//	{
	//		//startRecordingButton.enabled = available;
	//		if (!available) {
	//			SpeechToTextResultText.text = "Speech Recognition not available";
	//		} else {
	//			SpeechToTextResultText.text = "Say something :-)";
	//		}
	//	}
	//
	//	public void OnAuthorizationStatusFetched (AuthorizationStatus status)
	//	{
	//		SpeechToTextResultText.text = " OnAuthorizationStatusFetched ";
	//		switch (status) {
	//		case AuthorizationStatus.Authorized:
	//			//startRecordingButton.enabled = true;
	//			break;
	//		default:
	//			//startRecordingButton.enabled = false;
	//			SpeechToTextResultText.text = "Cannot use Speech Recognition, authorization status is " + status;
	//			break;
	//		}
	//	}
	//
	//	public void OnEndOfSpeech ()
	//	{
	//		SpeechToTextResultText.text = " OnEndOfSpeech ";
	//		Debug.Log ("UNITY End Result");
	//
	//		//startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
	//	}
	//
	//	public void OnError (string error)
	//	{
	//		Debug.LogError (error);
	//		SpeechToTextResultText.text = "Something went wrong... Try again! \n [" + error + "]";
	//		//startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
	//	}
	//
	//	public void OnStartRecordingPressed ()
	//	{
	//		SpeechToTextResultText.text = " OnStartRecordingPressed ";
	//		if (SpeechRecognizer.IsRecording ()) {
	//			SpeechRecognizer.StopIfRecording ();
	//			//startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
	//		} else {
	//			SpeechRecognizer.StartRecording (true);
	//			//startRecordingButton.GetComponentInChildren<Text>().text = "Stop Recording";
	//			SpeechToTextResultText.text = "Say something :-)";
	//		}
	//	}
	//
	//	void ActionsOnSpeechToTextResults (string Text)
	//	{
	//
	//		SpeechToTextResultText.text = Text;
	//
	//		if (Text.ToLower ().Equals (LanguageManager.Instance.GetTextValue (StaticTextAndValues.EXIT))) {
	//			Application.Quit ();
	//		} else if (Text.ToLower ().Equals (LanguageManager.Instance.GetTextValue (StaticTextAndValues.MENU))) {
	//			AppManager.instance.HideAllPanels ();
	//			DashboardPanelManager.instance.ShowPanel ();
	//		} else if (Text.ToLower ().Equals (LanguageManager.Instance.GetTextValue (StaticTextAndValues.RESTART))) {
	//			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	//		} else if (AppManager.instance.myPreferencesData.totalNumOfSubscription != 0) {
	//			if (Text.ToLower ().Equals (LanguageManager.Instance.GetTextValue ("Home").ToLower ())) {
	//
	//				AppManager.instance.HideAllPanels ();
	//				DashboardPanelManager.instance.ShowPanel ();
	//
	//			} else if (Text.ToLower ().Equals (LanguageManager.Instance.GetTextValue ("Analytics").ToLower ())) {
	//
	//				AppManager.instance.HideAllPanels ();
	//				AnalyticsPanelManager.instance.ShowPanel ();
	//			} else if (Text.ToLower ().Equals (LanguageManager.Instance.GetTextValue ("Search").ToLower ())) {
	//				AppManager.instance.HideAllPanels ();
	//				SearchPanelManager.instance.ShowPanel ();
	//			} else if (Text.ToLower ().Equals (LanguageManager.Instance.GetTextValue ("Settings").ToLower ())) {
	//				AppManager.instance.HideAllPanels ();
	//				SettingsPanelManager.instance.ShowPanel ();
	//			}
	//		}
	//
	//		switch (AppManager.instance.currentShowingPanel) {
	//
	//		case CURRENT_SHOWING_PANEL.GOOGLE_SIGN_IN:
	//			if (Text.ToLower () == "login with google") {
	//				LCGoogleSignInExample.instance.ManualInit ();
	//
	//			} else if (Text.ToLower () == "normal login") {
	//				MySignInPanelManager.instance.LoginButtonClicked ();
	//			}
	//			break;
	//		case CURRENT_SHOWING_PANEL.SETTINGS:
	//
	//			if (Text.ToLower ().Equals (LanguageManager.Instance.GetTextValue ("App Info").ToLower ())) {
	//
	//				AppManager.instance.HideAllPanels ();
	//				SettingsPanelManager.instance.GoToAppInfoPanel ();
	//
	//			} else if (Text.ToLower ().Equals (LanguageManager.Instance.GetTextValue ("Log Out").ToLower ())) {
	//
	//				AppManager.instance.HideAllPanels ();
	//				SettingsPanelManager.instance.LogOutButtonClicked ();
	//
	//			} else if (Text.ToLower ().Equals (LanguageManager.Instance.GetTextValue ("Choose Language").ToLower ())) {
	//
	//				AppManager.instance.HideAllPanels ();
	//				LanguageSelectionManager.instance.TableEnableDisable (true);
	//			}
	//			break;
	//
	//		case CURRENT_SHOWING_PANEL.SEARCH:
	//		case CURRENT_SHOWING_PANEL.SEARCH_AND_STORE_SEARCH:
	//
	//			if (SearchPanelManager.instance.isVoiceClicked) {
	//
	//				SearchPanelManager.instance.OnVoiceSearch (Text);
	//			}
	//			break;
	//		}
	//	}

	#endregion

}
