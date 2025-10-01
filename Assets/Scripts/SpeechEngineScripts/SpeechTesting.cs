using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;

public class SpeechTesting : MonoBehaviour
{
	//	public static SpeechTesting instance;

	[TextArea (5, 10)]
	public string conversation;

	public string userResponse;

	string path = "Assets/Resources/HistoryUserResponse.txt";

	[TextArea (5, 10)]
	public string pastUserResponses;

	SpeechRecognizationManager speechRecognizationManager;
	ChatbotManager chatBotManager;
	AlexaManager alexaManager;

	void Awake ()
	{
//		if (instance != null && instance != this) {
//			Destroy (instance.gameObject);
//			instance = null;
//		} 
//		if (instance == null) {
//			instance = this;
//			DontDestroyOnLoad (gameObject);
//		}
	}

	void Start ()
	{
		alexaManager = GetComponent<AlexaManager> ();
		chatBotManager = GetComponent<ChatbotManager> ();
		speechRecognizationManager = GetComponent<SpeechRecognizationManager> ();

		#if UNITY_EDITOR
		pastUserResponses = ReadFromUserResponseHistory ();
		#endif

	}

	public void SayAlexa ()
	{
		conversation = "";
		userResponse = "";

		alexaManager.onListenerSuccessfulmatch ();
	}

	public void AskAlexa ()
	{
		if (chatBotManager.chatBotState != ChatbotState.Active) {
		
			SayAlexa ();
	
		} else {
			
			speechRecognizationManager.OnFinalResult (userResponse);
			AddToConversation (userResponse, true);
		}
	}

	public void AddToConversation (string text, bool isUser)
	{
		if (isUser) {
			
			conversation = "\n" + "USER RESPONSE :: \n" + text;
			pastUserResponses += text + "\n";

			#if UNITY_EDITOR
			AddToUserResponseHistory (text);
			#endif

		} else {

			conversation += "\n\n" + "DIALOG FLOW RESPONSE :: \n" + text;
			userResponse = "";
		}
	}

	void AddToUserResponseHistory (string text)
	{
		StreamWriter writer = new StreamWriter (path, true);
		writer.Close ();

		if (ReadFromUserResponseHistory () != "") {
			string[] list = ReadFromUserResponseHistory ().Split ('\n');

			foreach (string s in list) {
				if (s == text) {
					return;
				}
			}
		}
		writer = new StreamWriter (path, true);

		writer.WriteLine (text + "\n");
		writer.Close ();
	}

	string ReadFromUserResponseHistory ()
	{
		StreamReader reader = new StreamReader (path);

		string fileText = reader.ReadToEnd ();
		reader.Close ();

		return fileText;
	}
}

