using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public enum ChatbotState
{
    Active,
    NotActive}
;

public enum ChatbotIcons
{
    NoIcon,
    Listening,
    Speaking,
    Processing}
;

public class ChatbotManager : MonoBehaviour
{
    public static ChatbotManager Instance;
    public ChatbotState chatBotState = ChatbotState.NotActive;
    public ChatbotIcons chatbotIcons = ChatbotIcons.NoIcon;

    public ScrollRect scrollRect;
    public Transform dialogContent;
    public GameObject ChatbotGameobject;
    public GameObject ListeningChatbot;
    public GameObject ProcessingChatbot;

    VoiceAssistanceStaticData voiceStaticData;
    ReverseChatScroll reverseChatScroll;
    ApiAiCustomResponseManager apiAiCustomResponseManager;
    VoiceAssistanceHandler voiceAssistanceHandler;
    VoiceAssistanceManager voiceAssistanceManager;


    void Start()
    {
        Instance = this;
        voiceStaticData = GetComponent<VoiceAssistanceStaticData>();
        reverseChatScroll = GetComponent<ReverseChatScroll>();
        apiAiCustomResponseManager = GetComponent<ApiAiCustomResponseManager>();
        voiceAssistanceHandler = GetComponent<VoiceAssistanceHandler>();
        voiceAssistanceManager = GetComponent<VoiceAssistanceManager>();
    }

    internal void ChatbotManager_OnStateOff(bool IsError)
    {
        DeactivateChatbot(IsError);
    }

    internal void ChatbotManager_OnStateListening()
    {
        chatbotIcons = ChatbotIcons.Listening;
        ListeningSpeakingState();
    }

    internal void ChatbotManager_OnStateSpeaking()
    {
        chatbotIcons = ChatbotIcons.Speaking;
        ListeningSpeakingState();
    }

    void ListeningSpeakingState()
    {
        ProcessingChatbot.GetComponent<ParticleSystem>().Stop();
        ProcessingChatbot.SetActive(false);
        ListeningChatbot.SetActive(true);
        ListeningChatbot.GetComponent<ParticleSystem>().Play();
    }

    internal void ChatbotManager_OnStateProcessing()
    {
        chatbotIcons = ChatbotIcons.Processing;
        ListeningChatbot.GetComponent<ParticleSystem>().Stop();
        ListeningChatbot.SetActive(false);
        ProcessingChatbot.SetActive(true);
        ProcessingChatbot.GetComponent<ParticleSystem>().Play();
    }

    GameObject backmenuGO, MainMenuCanvas;

    public List<GameObject> hideObjects;

    public GameObject eventSystem;

    public void ActivateChatbot()
    {
        Debug.Log("ActivateChatbot");
        if (SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "LoadingScene")
        {
            if (BackMenu.instance.IsBackMenuEnabled)
            {
                BackMenu.instance.ToggleBackMenu();
            }
        }
        if (EventSystem.current)
            eventSystem = EventSystem.current.gameObject;

        if (eventSystem != null)
        {
            eventSystem.GetComponent<EventSystem>().enabled = false;
        }

        if (SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "LoadingScene")
        {
            StartCoroutine(HideObjects.instance.PauseSimulation());
        }
			
        chatBotState = ChatbotState.Active;
        ResetChatbotData();

        #if UNITY_EDITOR
        ChatbotGameobject.transform.DOScale(1, voiceStaticData.chatBotPanelRate_Editor_IN);
        #else
		ChatbotGameobject.transform.DOScale (1,voiceStaticData.chatBotPanelRate_Android_IN);
        #endif

    }

    public void DeactivateChatbot(bool IsError)
    {	

        gameObject.transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        if (SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "LoadingScene")
        {
            HideObjects.instance.PlaySimulation();
        }

        if (eventSystem != null)
        {
            eventSystem.GetComponent<EventSystem>().enabled = true;
        }

        chatBotState = ChatbotState.NotActive;
        chatbotIcons = ChatbotIcons.NoIcon;
        ProcessingChatbot.GetComponent<ParticleSystem>().Stop();
        ListeningChatbot.GetComponent<ParticleSystem>().Stop();
        ListeningChatbot.SetActive(false);
        ProcessingChatbot.SetActive(false);

        #if UNITY_EDITOR

        if (IsError)
            ChatbotGameobject.transform.DOScale(0, voiceStaticData.chatBotPanelRate_Editor_OUT);
        else
            ChatbotGameobject.transform.DOScale(0, voiceStaticData.chatBotPanelRate_Editor_OUT).OnComplete(OnComplete);

        #else

		if (IsError)
		ChatbotGameobject.transform.DOScale (0,voiceStaticData.chatBotPanelRate_Android_OUT);
		else {
		if (voiceAssistanceManager.currentVoiceAction != TypeOfVoiceActions.settingSelection) {

		// Above checks are done so that doscaling occurs after in volume case
		ChatbotGameobject.transform.DOScale (0, voiceStaticData.chatBotPanelRate_Android_OUT).OnComplete (OnComplete);

		} else {
		if((apiAiCustomResponseManager.currentResponse.Result.Parameters ["change"].ToString ()=="increase")||(apiAiCustomResponseManager.currentResponse.Result.Parameters ["change"].ToString ()=="decrease"))
		voiceAssistanceHandler.DoAction ();
		else
		ChatbotGameobject.transform.DOScale (0, voiceStaticData.chatBotPanelRate_Android_OUT).OnComplete (OnComplete);
		}
		}

        #endif
    }

    public void OnComplete()
    {
        voiceAssistanceHandler.DoAction();
    }

    void ResetChatbotData()
    {
        foreach (Transform t in reverseChatScroll.dialogsParent.GetComponentsInChildren<Transform>())
        {
            if (!t.CompareTag("DialogParent"))
            {
                Destroy(t.gameObject);
            }
        }

        scrollRect.verticalScrollbar.value = 0;
        ProcessingChatbot.SetActive(false);
        ListeningChatbot.SetActive(false);
    }

    public void InitDialogForm()
    {
        scrollRect.verticalScrollbar.value = 0;
    }

    public void InitUserResponse()
    {
        reverseChatScroll.AddDialog(true);
    }

    public void OnUserResponse(string ResponseText)
    {
        MakeDialog(ResponseText, true);
    }

    public void InitChatbotResponse()
    {
        reverseChatScroll.AddDialog(false);
    }

    public void OnChatbotResponse(string ResponseText)
    {
        MakeDialog(ResponseText, false);
    }

    void MakeDialog(string ResponseText, bool IsUser)
    {
        if (IsUser)
        {
            reverseChatScroll.currUserDialog.GetComponentInChildren <Text>().text = /*"User : " +*/ ResponseText;

        }
        else
        {

            ProcessResponse(ResponseText);

        }
    }

    public void OnResultError()
    {
        //		reverseChatScroll.currUserDialog.GetComponentInChildren <Text> ().text = "Something Went Wrong"LanguageManager.Instance.GetTextValue ("ErrorMsg2");

        reverseChatScroll.currUserDialog.GetComponentInChildren <Text>().text = "Something Went Wrong";
        //reverseChatScroll.currResponseDialog.GetComponentInChildren <Text> ().text = "...";
    }

    void ProcessResponse(string ResponseText)
    {
        SetResponseText(ResponseText);

    }


    void NavigateResponse(string ResponseText)
    {
        SetResponseText(ResponseText);

    }

    void SettingsResponse(string ResponseText)
    {
        if (apiAiCustomResponseManager.currentResponse.Result.Parameters["setting"].ToString() == "volume")
        {

            SetResponseText(ResponseText);

            //		} else if (apiAiCustomResponseManager.currentResponse.Result.Parameters ["setting"].ToString () == "language") {
            //
            //			if (apiAiCustomResponseManager.currentResponse.Result.Parameters ["language"].ToString () != "") {
            //				
            //				if (voiceAssistanceHandler.CheckLanguageIfAvailable (apiAiCustomResponseManager.currentResponse.Result.Parameters ["language"].ToString ())) {
            //
            //					SetResponseText (ResponseText);
            //				
            //				} else {
            //				
            //					SetResponseText ("Language not available");
            //				
            //				}
            //
            //			} else {
            //				
            //				SetResponseText (ResponseText);
            //			}
            //
            //		} else {
        }	
        SetResponseText(ResponseText);

    }

    public void SetResponseText(string ResponseText)
    {
        voiceAssistanceManager.currentResponseText = /*"Response : " +*/ ResponseText;

        reverseChatScroll.currResponseDialog.GetComponentInChildren <Text>().text = voiceAssistanceManager.currentResponseText;
    }

    void OtherSceneResponse(string ResponseText)
    {

        SetResponseText("Only 'GO Back' allowed here");

    }


}
