using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Threading;

public class ReverseChatScroll : MonoBehaviour
{
	//	public static ReverseChatScroll instance;

	public float spacing = 0f;
	public float dialogHeight = 160f;
	public float topSpacing = 20f;
	public float bottomSpacing = 20f;
	public int noOfDialogsAtOnce = 2;
	public float userObjSpacingLeft = 20f;
	public float responseObjSpacingRight = 20f;

	public RectTransform scrollViewRect;
	public RectTransform contentRect;
	public RectTransform dialogsParent;

	public GameObject userObj;
	public GameObject responseObj;

	float startingPosY;
	float currPosY;

	internal GameObject currUserDialog;
	internal GameObject currResponseDialog;

	internal string userResponseToBeSend;

	SpeechRecognizationManager speechRecognizationManager;
	VoiceAssistanceStaticData voiceStaticData;
	VoiceAssistanceManager voiceAssistanceManager;

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
		voiceStaticData = GetComponent<VoiceAssistanceStaticData> ();
		voiceAssistanceManager = GetComponent<VoiceAssistanceManager> ();
		speechRecognizationManager = GetComponent<SpeechRecognizationManager> ();

		scrollViewRect.sizeDelta = new Vector2 (scrollViewRect.sizeDelta.x, dialogHeight * noOfDialogsAtOnce + topSpacing + spacing + bottomSpacing);

		startingPosY = dialogHeight / 2f;
		currPosY = 0;
	}

	public void AddDialog (bool isUserResponse)
	{
		contentRect.offsetMin = new Vector2 (spacing, bottomSpacing);
		contentRect.offsetMax = new Vector2 (-spacing, 0);

		if (dialogsParent.childCount > 0) {
			currPosY += startingPosY;

			if (currPosY >= noOfDialogsAtOnce * dialogHeight) {
				contentRect.offsetMin = new Vector2 (spacing, 0);
				contentRect.offsetMax = new Vector2 (-spacing, (currPosY - ((noOfDialogsAtOnce * 2 - 1) * dialogHeight / 2f)));
			}

			MakeDialogObj (isUserResponse);

			StartCoroutine (DOMoveForRectTransform (dialogsParent, currPosY, 0.5f, isUserResponse));
		} else {
			MakeDialogObj (isUserResponse);
		}
	}

	void MakeDialogObj (bool isUserResponse)
	{
		if (isUserResponse) {
			currUserDialog = Instantiate (userObj)as GameObject;
			ResetDialogObj (currUserDialog, true);

		} else {
			currResponseDialog = Instantiate (responseObj)as GameObject;
			ResetDialogObj (currResponseDialog, false);
		}
	}

	void ResetDialogObj (GameObject obj, bool isUser)
	{
		obj.transform.SetParent (dialogsParent);
		obj.transform.SetAsLastSibling ();

		obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.Euler (Vector3.zero);

		obj.GetComponent<RectTransform> ().localPosition = new Vector3 (obj.GetComponent<RectTransform> ().localPosition.x, 0, 0);
		obj.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (obj.GetComponent<RectTransform> ().anchoredPosition.x, -currPosY);

		if (isUser) {
			obj.GetComponent<RectTransform> ().offsetMax = new Vector2 (0, obj.GetComponent<RectTransform> ().offsetMax.y);
			obj.GetComponent<RectTransform> ().offsetMin = new Vector2 (userObjSpacingLeft, obj.GetComponent<RectTransform> ().offsetMin.y);

		} else {
			obj.GetComponent<RectTransform> ().offsetMax = new Vector2 (-responseObjSpacingRight, obj.GetComponent<RectTransform> ().offsetMax.y);
			obj.GetComponent<RectTransform> ().offsetMin = new Vector2 (0, obj.GetComponent<RectTransform> ().offsetMin.y);
		}

		obj.GetComponentInChildren <Text> ().text = "...";
		obj.GetComponentInChildren <Text> ().fontSize = voiceStaticData.chatBotFontSize;
		obj.SetActive (true);
	}

	IEnumerator DOMoveForRectTransform (RectTransform rectTransform, float endValue, float rate, bool isUserResponse)
	{
		bool reached = false;
		float currentY = rectTransform.anchoredPosition.y;

		while (!reached) {
			rectTransform.anchoredPosition = new Vector2 (0, currentY); 

			currentY += voiceStaticData.chatSpeed;

			if (currentY > endValue)
				reached = true;
			
			yield return null;
		}

		if (speechRecognizationManager.playedOnce) {
			if (!isUserResponse) {

				if (voiceAssistanceManager.currentVoiceAction == TypeOfVoiceActions.noAction) {

					// --------------- SENDING USER RESPONSE ------------

		//			#if UNITY_ANDROID && !UNITY_EDITOR
		//				speechRecognizationManager.CallDialogThread (userResponseToBeSend);
		//			#else
					speechRecognizationManager.SendTextToDialogFlow (userResponseToBeSend);
		//			#endif
				}
			}
		}
	}
}
