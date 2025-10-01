using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using KKSpeech;

public class RecordingCanvas : MonoBehaviour {

	//public Button startRecordingButton;
	public Text resultText;
	public static RecordingCanvas instance;

	void Awake(){

		instance = this;
	}
	void Start() {
		
		if (SpeechRecognizer.ExistsOnDevice()) {
			resultText.text = " ExistsOnDevice ";
			SpeechRecognizerListener listener = GameObject.FindObjectOfType<SpeechRecognizerListener>();
			listener.onAuthorizationStatusFetched.AddListener(OnAuthorizationStatusFetched);
			listener.onAvailabilityChanged.AddListener(OnAvailabilityChange);
			listener.onErrorDuringRecording.AddListener(OnError);
			listener.onErrorOnStartRecording.AddListener(OnError);
			listener.onFinalResults.AddListener(OnFinalResult);
			listener.onPartialResults.AddListener(OnPartialResult);
			listener.onEndOfSpeech.AddListener(OnEndOfSpeech);
			//startRecordingButton.enabled = false;
			SpeechRecognizer.RequestAccess();
		} else {
			resultText.text = "Sorry, but this device doesn't support speech recognition";
			//startRecordingButton.enabled = false;
		}

	}

	public void OnFinalResult(string result) {
		resultText.text = " OnFinalResult ";
		Debug.Log ("UNITY Final Result");
		resultText.text = result;
	}

	public void OnPartialResult(string result) {
		resultText.text = " OnPartialResult ";
		Debug.Log ("UNITY Partial Result");
		resultText.text = result;
	}

	public void OnAvailabilityChange(bool available) {
		resultText.text = " OnAvailabilityChange ";
		//startRecordingButton.enabled = available;
		if (!available) {
			resultText.text = "Speech Recognition not available";
		} else {
			resultText.text = "Say something :-)";
		}
	}

	public void OnAuthorizationStatusFetched(AuthorizationStatus status) {
		resultText.text = " OnAuthorizationStatusFetched ";
		switch (status) {
		case AuthorizationStatus.Authorized:
			//startRecordingButton.enabled = true;
			break;
		default:
			//startRecordingButton.enabled = false;
			resultText.text = "Cannot use Speech Recognition, authorization status is " + status;
			break;
		}
	}

	public void OnEndOfSpeech() {
		resultText.text = " OnEndOfSpeech ";
		Debug.Log ("UNITY End Result");
		//startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
	}

	public void OnError(string error) {
		Debug.LogError(error);
		resultText.text = "Something went wrong... Try again! \n [" + error + "]";
		//startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
	}

	public void OnStartRecordingPressed() {
		resultText.text = " OnStartRecordingPressed ";
		if (SpeechRecognizer.IsRecording()) {
			SpeechRecognizer.StopIfRecording();
			//startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
		} else {
			SpeechRecognizer.StartRecording(true);
			//startRecordingButton.GetComponentInChildren<Text>().text = "Stop Recording";
			resultText.text = "Say something :-)";
		}
	}
}
