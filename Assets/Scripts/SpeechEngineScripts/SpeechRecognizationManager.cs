#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Reflection;
using ApiAiSDK;
using ApiAiSDK.Model;
using ApiAiSDK.Unity;
using Newtonsoft.Json;
using System.Net;
using LitJson;
using InfinityEngine.Localization;
using KKSpeech;
using System.Threading;

using UnityEngine.SceneManagement;

#endregion

public enum SpeechEngineState
{
	Off,
	Listening,
	Speaking,
	Processing}
;

public class SpeechRecognizationManager : MonoBehaviour
{
	#region Variables

	public static SpeechRecognizationManager Instance;

	public static event Action<bool> OnStateOff = null;
	public static event Action OnStateListening = null;
	public static event Action OnStateSpeaking = null;
	public static event Action OnStateProcessing = null;

	public SpeechEngineState speechEngineState;
	public Text MessageText;

	private ApiAiUnity apiAiUnity;

	private readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings { 
		NullValueHandling = NullValueHandling.Ignore,
	};

	private readonly Queue<Action> ExecuteOnMainThread = new Queue<Action> ();

	private TTSEngine[] engines;
	private Locale[] locales;
	private Voice[] voices;

	internal bool playedOnce;

	AudioClip clip;
	public Thread DialogFlowThread;

	ChatbotManager chatBotManager;
	SpeechTesting speechTesting;
	ReverseChatScroll reverseChatScroll;
	ApiAiCustomResponseManager apiAiCustomResponseManager;
	AlexaManager alexaManager;
	VoiceAssistanceHandler voiceAssistanceHandler;
	VoiceAssistanceManager voiceAssistanceManager;

	#endregion

	#region Start

	void Awake ()
	{
		if (Instance != null && Instance != this) {
			Destroy (this);
			Instance = null;
		} 
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);

			OnStateOff += SpeechRecognizationManager_OnStateOff;
			OnStateListening += SpeechRecognizationManager_OnStateListening;
			OnStateSpeaking += SpeechRecognizationManager_OnStateSpeaking;
			OnStateProcessing += SpeechRecognizationManager_OnStateProcessing;
		} 

	}

	void SpeechRecognizationManager_OnStateOff (bool IsError)
	{
		speechEngineState = SpeechEngineState.Off;
		chatBotManager.ChatbotManager_OnStateOff (IsError);
	}

	void SpeechRecognizationManager_OnStateListening ()
	{
		speechEngineState = SpeechEngineState.Listening;
		chatBotManager.ChatbotManager_OnStateListening ();
	}

	void SpeechRecognizationManager_OnStateSpeaking ()
	{
		speechEngineState = SpeechEngineState.Speaking;
		chatBotManager.ChatbotManager_OnStateSpeaking ();
	}

	void SpeechRecognizationManager_OnStateProcessing ()
	{
		speechEngineState = SpeechEngineState.Processing;
		chatBotManager.ChatbotManager_OnStateProcessing ();
	}


	IEnumerator Start ()
	{
		 if (SpeechRecognizer.ExistsOnDevice() )
            {

            reverseChatScroll = GetComponent<ReverseChatScroll>();
            apiAiCustomResponseManager = GetComponent<ApiAiCustomResponseManager>();
            voiceAssistanceHandler = GetComponent<VoiceAssistanceHandler>();
            voiceAssistanceManager = GetComponent<VoiceAssistanceManager>();
            alexaManager = GetComponent<AlexaManager>();
            chatBotManager = GetComponent<ChatbotManager>();
            speechTesting = GetComponent<SpeechTesting>();
            AddAlexaListeners();
            InitTextToSpeech();
            // check access to the Microphone
            yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
            InitDialogFlow();
            InitSpeechToText();

#if UNITY_ANDROID && !UNITY_EDITOR
		alexaManager.Init ();
#endif
        }
#if UNITY_EDITOR
        reverseChatScroll = GetComponent<ReverseChatScroll>();
        apiAiCustomResponseManager = GetComponent<ApiAiCustomResponseManager>();
        voiceAssistanceHandler = GetComponent<VoiceAssistanceHandler>();
        voiceAssistanceManager = GetComponent<VoiceAssistanceManager>();
        alexaManager = GetComponent<AlexaManager>();
        chatBotManager = GetComponent<ChatbotManager>();
        speechTesting = GetComponent<SpeechTesting>();
        AddAlexaListeners();
        InitTextToSpeech();
        // check access to the Microphone
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        InitDialogFlow();
        InitSpeechToText();

#if UNITY_ANDROID && !UNITY_EDITOR
		alexaManager.Init ();
#endif
#endif
	}

	void Update ()
	{
		if (apiAiUnity != null) {
			apiAiUnity.Update ();
		}

		while (ExecuteOnMainThread.Count > 0) {
			ExecuteOnMainThread.Dequeue ().Invoke ();
		}
	}

	#endregion


	#region Alexa

	void AddAlexaListeners ()
	{
		AlexaManager.OnInitService += AlexaManager_OnInitService;
		AlexaManager.OnInitSerivceError += AlexaManager_OnInitSerivceError;
		AlexaManager.OnStartService += AlexaManager_OnStartService;
		AlexaManager.OnStartServiceError += AlexaManager_OnStartServiceError;

		AlexaManager.OnStopService += AlexaManager_OnStopService;
		AlexaManager.OnStopServiceError += AlexaManager_OnStopServiceError;

		AlexaManager.OnHotwordSuccesfullMatch += AlexaManager_OnHotwordSuccesfullMatch;

	}


	void AlexaManager_OnInitService ()
	{

		MessageText.text = "Initializing";
		alexaManager.StartService ();
	}

	void AlexaManager_OnInitSerivceError ()
	{

		MessageText.text = "Alexa Intializing Error..";
	}

	void AlexaManager_OnStartService ()
	{
		MessageText.text = "Say Alexa to activate";
	}

	void AlexaManager_OnStartServiceError ()
	{
		MessageText.text = "Could Not Start Alexa";
	}


	void AlexaManager_OnStopService ()
	{
	}

	void AlexaManager_OnStopServiceError ()
	{
		MessageText.text = "Could Not Stop Alexa";
	}
		

	void AlexaManager_OnHotwordSuccesfullMatch ()
	{

		alexaManager.StopService ();

		chatBotManager.ActivateChatbot ();

//		GameObject gp = Camera.main.transform.root.gameObject ;
		GameObject gp = dpn.DpnCameraRig._instance._center_eye.gameObject;
		gameObject.transform.SetParent (gp.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localEulerAngles = Vector3.zero;

		MessageText.text = "Alexa Activated , Ask a question";
		StartCoroutine (ProcessAfterAlexaActivates ());
	}

	IEnumerator ProcessAfterAlexaActivates ()
	{
		yield return new WaitForSeconds (0.2f);

		if (!playedOnce) {

			string beginningResponse = "";
			reverseChatScroll.AddDialog (false);

			if (DateTime.Now.Hour < 12) {
				beginningResponse = "Hi Good morning!";

				//				beginningResponse = LanguageManager.Instance.GetTextValue ("GoodMorning");
			} else if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 16) {
				beginningResponse = "Hi Good afternoon!";
				;
			} else if (DateTime.Now.Hour >= 16) {
				beginningResponse = "Hi Good evening!";

			}

			chatBotManager.SetResponseText (beginningResponse);
			SpeechEngine.Speak (beginningResponse);
		}

		if (!playedOnce) {
			while (SpeechEngine.IsSpeaking) {
				yield return null;
			}
			playedOnce = true;
		}

		OnStartRecording ();
	}


	#endregion


	#region SpeechToText

	void InitSpeechToText ()
	{
		if (SpeechRecognizer.ExistsOnDevice ()) {
			SpeechRecognizerListener listener = GameObject.FindObjectOfType<SpeechRecognizerListener> ();
			listener.onAuthorizationStatusFetched.AddListener (OnAuthorizationStatusFetched);
			listener.onAvailabilityChanged.AddListener (OnAvailabilityChange);
			listener.onErrorDuringRecording.AddListener (OnError);
			listener.onErrorOnStartRecording.AddListener (OnError);
			listener.onFinalResults.AddListener (OnFinalResult);
			listener.onPartialResults.AddListener (OnPartialResult);
			listener.onEndOfSpeech.AddListener (OnEndOfSpeech);
			SpeechRecognizer.RequestAccess ();
		} else {
			Debug.LogError ("Sorry, but this device doesn't support speech recognition");
		}
	}

	public void OnAuthorizationStatusFetched (AuthorizationStatus status)
	{
		switch (status) {
		case AuthorizationStatus.Authorized:
			break;
		default:
			Debug.LogError ("Cannot use Speech Recognition, authorization status is " + status);

			break;
		}
	}

	public void  OnFinalResult (string result)
	{
		StartCoroutine (DelayDialogflowDataSend (result));
	}

	IEnumerator DelayDialogflowDataSend (string result)
	{
		voiceAssistanceManager.currentVoiceAction = TypeOfVoiceActions.noAction;

		chatBotManager.OnUserResponse (result);
		chatBotManager.InitChatbotResponse ();

		yield return new WaitForEndOfFrame ();

		SpeechRecognizationManager_OnStateProcessing ();

		reverseChatScroll.userResponseToBeSend = result;
	}

	public void OnPartialResult (string result)
	{
		chatBotManager.OnUserResponse (result);
	}

	public void OnAvailabilityChange (bool available)
	{
		if (!available) {
			Debug.LogError ("Speech Recognition not available");

		} else {
			Debug.Log ("Say something :-)");


		}
	}

	public void OnEndOfSpeech ()
	{
		MessageText.text = "Processing...";
		SpeechRecognizationManager_OnStateProcessing (); 
	}

	public void OnError (string error)
	{
		chatBotManager.OnResultError ();
		SpeechRecognizationManager_OnStateOff (true);
		alexaManager.StartService ();
		Debug.LogError ("Something went wrong... Try again! \n [" + error + "]");
	}


	public void OnStartRecording ()
	{
		chatBotManager.InitDialogForm ();
		AfterInitDialogForm ();
	}

	public void AfterInitDialogForm ()
	{
		chatBotManager.InitUserResponse ();

		SpeechRecognizationManager_OnStateListening ();

		if (SpeechRecognizer.IsRecording ()) {
			SpeechRecognizer.StopIfRecording ();
		} else {
			SpeechRecognizer.StartRecording (true);
		}
	}

	#endregion


	#region DialogFlow

	//	AIResponse aiResponse;
	internal APIAICustomResponse aiResponse;

	//	JsonData ItemName;

	void InitDialogFlow ()
	{

		if (!Application.HasUserAuthorization (UserAuthorization.Microphone)) {
			throw new NotSupportedException ("Microphone using not authorized");
		}


		ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => {
			return true;
		};

		const string ACCESS_TOKEN = "38ab6da965ed4f0dbdab9edcb98e11ba";

		var config = new AIConfiguration (ACCESS_TOKEN, SupportedLanguage.English);
		apiAiUnity = new ApiAiUnity ();
		apiAiUnity.Initialize (config);

		apiAiUnity.OnError += HandleOnError;
		apiAiUnity.OnResult += HandleOnResult;
	}

	public void CallDialogThread (string text)
	{
		DialogFlowThread = new Thread (() => SendTextToDialogFlow (text));
		DialogFlowThread.Start ();
	}

	public void SendTextToDialogFlow (string Text)
	{
		var text = Text;

		SpeechRecognizationManager_OnStateProcessing ();

		apiAiCustomResponseManager.TextRequest (text);

		StartCoroutine (CheckForRequestProcessed ());

	}

	IEnumerator CheckForRequestProcessed ()
	{
		while (apiAiCustomResponseManager.currentState == ApiAiCustomResponseManagerState.PROCESSING) {
			yield return null;
		}

		if (apiAiCustomResponseManager.currentState == ApiAiCustomResponseManagerState.SUCCESS) {

			aiResponse = apiAiCustomResponseManager.currentResponse;

			voiceAssistanceHandler.SetAction ();

			if (aiResponse != null) {
				// to override result based on condition writtern in voice assistant handler--> Scene navigation
				if (voiceAssistanceManager.currentVoiceAction == TypeOfVoiceActions.navigateScene ) {
					voiceAssistanceHandler.IsNavigateScenesExists ();
				} 
				// to override result based on condition writtern in voice assistant handler--> Action navigation
				if (voiceAssistanceManager.currentVoiceAction == TypeOfVoiceActions.navigateActions ) {
					voiceAssistanceHandler.IsNavigateActions ();
				} 
				// to override result based on condition writtern in voice assistant handler--> Back menu navigation
				if (voiceAssistanceManager.currentVoiceAction == TypeOfVoiceActions.navigateBackMenu ) {
					voiceAssistanceHandler.isBackMenuEnabled ();
				} 
				// to override result based on condition writtern in voice assistant handler--> Exit navigation
				if (voiceAssistanceManager.currentVoiceAction == TypeOfVoiceActions.confirmation_yes ) {
					voiceAssistanceHandler.isQuitable ();
				} 
				StartCoroutine (OnDialogFlowResult ());

			} else {
				Debug.LogError ("Response is null");

				OnErrorFromDialogFlow ();
			}

		} else if (apiAiCustomResponseManager.currentState == ApiAiCustomResponseManagerState.FAILURE) {
			Debug.LogError ("API Failure");
		}
	}

	internal void OnErrorFromDialogFlow ()
	{
		StartCoroutine (OnDialogFlowResultError ("Sorry , There is a network issue. Try Again"));
		MessageText.text = "Sorry , There is a network issue. Try Again";
	}


	public IEnumerator OnDialogFlowResult ()
	{
		SpeechRecognizationManager_OnStateSpeaking ();

		if (speechTesting != null)
			speechTesting.AddToConversation (aiResponse.Result.Fulfillment.Speech, false);

		chatBotManager.OnChatbotResponse (aiResponse.Result.Fulfillment.Speech);
		SpeechEngine.Speak (aiResponse.Result.Fulfillment.Speech);

		while (SpeechEngine.IsSpeaking) {
			yield return null;
		}

		yield return new WaitForEndOfFrame ();

		if (voiceAssistanceManager.currentVoiceAction == TypeOfVoiceActions.exitApp) {
			OnStartRecording ();

			yield break;
		}

		SpeechRecognizationManager_OnStateOff (false);
		alexaManager.StartService ();
	}

	void SpeakSearchResults ()
	{
		SpeechRecognizationManager_OnStateSpeaking ();

		MessageText.text = "Alexa Activated , Ask a question";
		OnStartRecording ();
	}

	public IEnumerator OnDialogFlowResultError (string Text)
	{
		SpeechRecognizationManager_OnStateSpeaking ();

		chatBotManager.OnChatbotResponse (Text);
		SpeechEngine.Speak (Text);

		while (SpeechEngine.IsSpeaking) {
			yield return null;
		}

		yield return new WaitForEndOfFrame ();

		SpeechRecognizationManager_OnStateOff (true);
		alexaManager.StartService ();
	}

	public void StartNativeRecognition ()
	{
		try { 
			apiAiUnity.StartNativeRecognition ();
		} catch (Exception ex) {
			Debug.LogException (ex);
		}
	}

	void HandleOnResult (object sender, AIResponseEventArgs e)
	{
		RunInMainThread (() => {

			aiResponse = (APIAICustomResponse)e.Response;

			if (aiResponse != null) {
				var outText = JsonConvert.SerializeObject (aiResponse, jsonSettings);
				Debug.Log (outText);
			} else {
				Debug.LogError ("Response is null");
			}
		});
	}

	void HandleOnError (object sender, AIErrorEventArgs e)
	{
		RunInMainThread (() => {

		});
	}

	private void RunInMainThread (Action action)
	{
		ExecuteOnMainThread.Enqueue (action);
	}

	#endregion


	#region TextToSpeech

	void InitTextToSpeech ()
	{
		Locale ar = new Locale ("Arabic", "ar-AR", "");

		SpeechEngine.AddCallback (() => {
			locales = Locale.AllLocales; 
			voices = SpeechEngine.AvaillableVoices;
			engines = SpeechEngine.AvailableEngines;

		});

	}

	#endregion

}
