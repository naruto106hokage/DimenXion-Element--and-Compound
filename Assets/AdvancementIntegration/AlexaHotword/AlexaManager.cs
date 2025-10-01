using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InfinityEngine.Localization;
using System;
using UnityEngine.SceneManagement;

public enum HotwordListeningState
{
	Active,
	NotActive}
;

public class AlexaManager : MonoBehaviour
{
	//	public static AlexaManager instance;

	public static event Action OnInitService = null;
	public static event Action OnInitSerivceError = null;
	public static event Action OnStartService = null;
	public static event Action OnStartServiceError = null;
	public static event Action OnStopService = null;
	public static event Action OnStopServiceError = null;
	public static event Action OnHotwordSuccesfullMatch = null;

	public HotwordListeningState HotwordState = HotwordListeningState.NotActive;

	string ClassPath = "broadcastplugin.umety.com.masterappbroadcast.DActivity";

	internal AndroidJavaObject ClassName;

	AndroidJavaClass unityPlayerClass;
	AndroidJavaObject currentActivity;
	internal AndroidJavaObject Context;

	bool IsFirstTime = true;

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
		GetAndroidObjects ();
	}

	void GetAndroidObjects ()
	{
		ClassName = new AndroidJavaObject (ClassPath);
		unityPlayerClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer"); 
		currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject> ("currentActivity");
		Context = currentActivity.Call<AndroidJavaObject> ("getApplicationContext");
	}

	void Start ()
	{
		
	}

	public void Init ()
	{
		try {
			ClassName.Call ("Init", new object[] { gameObject.name, Context });
			OnInitService ();
		} catch (Exception e) {
			OnInitSerivceError ();
			Debug.LogException (e);
		}
	}

	public void StartService ()
	{
		if (IsFirstTime)
			IsFirstTime = false;
		try {
			HotwordState = HotwordListeningState.Active;

			#if UNITY_ANDROID && !UNITY_EDITOR
				ClassName.Call ("startRecording");
			#endif

			OnStartService ();
		} catch (Exception e) {
			OnStartServiceError ();
			Debug.LogException (e);
		}
	}

	public void StopService ()
	{
		try {
			HotwordState = HotwordListeningState.NotActive;

			#if UNITY_ANDROID && !UNITY_EDITOR
				ClassName.Call ("stopRecording");
			#endif

			OnStopService ();
		} catch (Exception e) {
			OnStopServiceError ();
			Debug.LogException (e);
		}
	}

	//for callback from snowboy , gameobject name should be "AlexaManager"
	//and method name should be "onListenerSuccessfulmatch"

	public void onListenerSuccessfulmatch ()
	{
//		SpeechRecognizationManager.Instance.gameObject.transform.localRotation = Quaternion.Euler (new Vector3 (0, Camera.main.transform.localRotation.eulerAngles.y, 0));
		if(SceneManager.GetActiveScene().name=="LoadingScene")
			return;
		print ("onListenerSuccessfulmatch");
		OnHotwordSuccesfullMatch ();
	}

	public void SetMaxVolume ()
	{
		ClassName.Call ("setMaxVolume");
	}

	public void SetProperVolume ()
	{
		ClassName.Call ("setProperVolume");
	}

	public void RestoreVolume ()
	{
		ClassName.Call ("restoreVolume");
	}

	void OnApplicationPause (bool pauseStatus)
	{
		if (IsFirstTime)
			return;
		
		if (pauseStatus) {
			if (HotwordState == HotwordListeningState.Active) {
				StopService ();
				HotwordState = HotwordListeningState.Active;
			}
		} else {
			if (HotwordState == HotwordListeningState.Active)
				StartService ();
		}
			
	}

	public void StartNativeSpeechToText ()
	{
		ClassName.Call ("StartNativeSpeechRecognizer", Context);
	}

	void OnFinalResult (string Text)
	{
		print ("OnFinalResult " + Text);
	}

	void OnPartialResult (string Text)
	{
		print ("OnPartialResult " + Text);
	}

}
