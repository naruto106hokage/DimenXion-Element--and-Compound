using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlertPanelManager : MonoBehaviour 
{
    public static AlertPanelManager apm;
    private Text alertMessageText;
    public LocaliseTextAndVoiceOver _ltvo;

    void Awake() 
    { 
        apm = this;
        alertMessageText = transform.GetChild(0).GetComponentInChildren<Text>();
    }

    void Start()
    {
        if (transform.GetChild(0).GetComponent<LocaliseTextAndVoiceOver>() == null)
            gameObject.transform.GetChild(0).gameObject.AddComponent<LocaliseTextAndVoiceOver>();
        _ltvo = transform.GetChild(0).GetComponent<LocaliseTextAndVoiceOver>();
        _ltvo.localizeType = LocalizeType.TextAndVoice;
    }

    public void ShowAlertPanel() 
    {
        transform.GetChild(0).gameObject.SetActive(true);
        float audLen = GetAudioClipAndLength.instance.GetVoiceOverLength(transform.GetChild(0).name) + 1;
        Invoke("HideAlertPanel",audLen);
    }

    public void HideAlertPanel() 
    {
        VrSelector.instance.EnableUIRayCasters();
        VrSelector.instance.EnablePhysicsRayCasters();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ShowAlertMessage (string alertMessage) 
    {
        if (alertMessage == "Internal server error")
        {
            transform.GetChild(0).name = "C_Inst_ServerError";
        }
        else
        {
            transform.GetChild(0).name = "C_Inst_CheckNetworkConnection";
        }

        VrSelector.instance.DisableUIRayCasters();
        VrSelector.instance.DisablePhysicsRayCasters();

        Invoke ( "ShowAlertPanel" , 0.1f );
    }
}
