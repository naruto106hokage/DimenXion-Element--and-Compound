using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfVoiceActions
{
	exitApp,
	settingSelection,
	noAction,
	navigateScene,
	navigateActions,
	confirmation_yes,
	confirmation_no,
	restart,
	navigateBackMenu,
	other}
;

public class NavigateAction
{
	public enum Paramter
	{
		ScreenName,
		EventName}

	;

	public enum EventName
	{
		Open,
		Close,
		Back}

	;

	public string ScreenName;
}

public class VoiceAssistanceStaticData : MonoBehaviour
{

	public static VoiceAssistanceStaticData instance;

	[Header ("Chatbot Settings")]
	public int chatBotFontSize = 25;
	public float chatBotPanelRate_Editor_IN = 0.5f;
	public float chatBotPanelRate_Editor_OUT = 0.5f;

	public float chatBotPanelRate_Android_IN = 3f;
	public float chatBotPanelRate_Android_OUT = 3f;

	public float chatSpeed = 5f;

	[Header ("Volume Settings")]
	public int volumeChangeAmount = 2;
	public float volumeBarTotalWidth = 3.5f;

	void Start(){
		instance = this;
	}
}
