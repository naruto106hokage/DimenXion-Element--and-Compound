using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardboardControllerInput : MonoBehaviour {

	private static bool okButtonClicked  = false;
	private static bool okButtonDown = false;
	private static bool okButtonUp = false;
	private static bool okButtonPressed = false;


	public static CardboardControllerInput instance;


	/// If true, the ok button was just released. This is an event flag:
	/// it will be true for only one frame after the event happens.
	public static bool OkButtonClicked
	{
		get{ 
			return okButtonClicked;
		}
	}


	/// If true, the user just started pressing the ok button. This is an event flag (it is true
	/// for only one frame after the event happens, then reverts to false).
	public static bool OkButtonDown
	{
		get{ 
			return okButtonDown;
		}
	}

	/// If true, the ok button is currently being pressed. This is not
	/// an event: it represents the trigger's state (it remains true while the trigger is being
	/// pressed).
	public static bool OkButtonPressed
	{
		get{ 
			return okButtonPressed;
		}
	}

	/// If true, the ok button was just released. This is an event flag:
	/// it will be true for only one frame after the event happens.
	public static bool OkButtonUp
	{
		get{ 
			return okButtonUp;
		}
	}

	void Awake()
	{
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);	
		} else if (instance != this) {
			Destroy (this.gameObject);
			return;
		}
	}



	public void OnDPADCenterDown()
	{
		okButtonDown= true;
		okButtonPressed = true;
		StartCoroutine ("ResetOkButtonDownAtEndOfFrame");

	}

	public void OnDPADCenterUp()
	{
		okButtonClicked = true;
		okButtonUp= true;
		okButtonPressed = false;

		StartCoroutine ("ResetOkButtonAtEndOfFrame");
	}

	IEnumerator ResetOkButtonDownAtEndOfFrame()
	{
		yield return null;
		okButtonDown = false;
	}


    IEnumerator ResetOkButtonAtEndOfFrame()
    {
		yield return null;
        okButtonClicked = false;
		okButtonUp = false;
    }
}
