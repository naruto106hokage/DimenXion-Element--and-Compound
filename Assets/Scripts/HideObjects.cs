using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjects : MonoBehaviour
{

	public static HideObjects instance;

	public List<GameObject> hideObjects;
	public List<Vector3> _HideObjectsScale;
	public List<GameObject> disableObjects;
	public bool IsBackMenuEnabled;
	AudioSource[ ] _AllAudioSrc;
	List <bool> audioPlaying;
	GameObject bm;
	int hideObject=1;
	bool[] _DisableObjectsState;

	void Start () 
	{
		
		instance = this;
		audioPlaying = new List<bool> ();
		hideObjects = new List<GameObject> ();
		_HideObjectsScale = new List<Vector3> ();
		disableObjects=new List<GameObject>();
		bm = GameObject.Find ("BackMenu");

		if (bm == null)
			return;

		for (int i = 0; i < BackMenu.instance._HideObjects.Length; i++) 
		{
			hideObjects.Add(BackMenu.instance._HideObjects [i]);
		}

		for (int i = 0; i < BackMenu.instance._DisableObjects.Length; i++) 
		{
			disableObjects.Add(BackMenu.instance._DisableObjects[i]);

		}

	}

	public IEnumerator PauseSimulation(){

		_DisableObjectsState = new bool[disableObjects.Count];

		_AllAudioSrc = Resources.FindObjectsOfTypeAll < AudioSource >();
		audioPlaying.Clear();

		for (int i = 0; i < _AllAudioSrc.Length; i++)
		{
			if (_AllAudioSrc[i].isPlaying)
			{
				audioPlaying.Add(true);
				_AllAudioSrc[i].Pause();
			}
			else
				audioPlaying.Add(false);
		}

		if (hideObject == 1)
		{
			
			
			for (int i = 0; i < hideObjects.Count; i++) 
			{
				
				_HideObjectsScale.Add (hideObjects [i].transform.localScale);
			}
			hideObject = 2;
		}
			for (int i = 0; i < hideObjects.Count; i++) 
			{
				
				hideObjects [i].transform.localScale = Vector3.zero;
			}
			
		for (int i = 0; i < disableObjects.Count; i++)
		{
			_DisableObjectsState[i] = disableObjects[i].activeSelf;
		}

			for (int i = 0; i <disableObjects.Count; i++)
			{
				
			   disableObjects[i].SetActive(false);
			}

		yield return new WaitForSeconds (VoiceAssistanceStaticData.instance.chatBotPanelRate_Editor_IN);
		Time.timeScale = 0;
	}

	public void PlaySimulation(){

		Time.timeScale = 1;
		_AllAudioSrc = Resources.FindObjectsOfTypeAll < AudioSource >();

		if (_AllAudioSrc.Length == 0)
			return;

		if (audioPlaying.Count == 0)
			return;

		for (int i = 0; i < _AllAudioSrc.Length; i++)
		{
			if (audioPlaying[i])
			{
				_AllAudioSrc[i].UnPause();
			}
		}

		for (int i = 0; i < hideObjects.Count; i++)
		{
			hideObjects [i].transform.localScale = _HideObjectsScale [i];
		}



		for (int i = 0; i <disableObjects.Count; i++)
		{
			
			disableObjects[i].SetActive(_DisableObjectsState[i]);
		}
	
	}
}
