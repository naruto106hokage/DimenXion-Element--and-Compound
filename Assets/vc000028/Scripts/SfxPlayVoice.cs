using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxPlayVoice : MonoBehaviour
{

	public static SfxPlayVoice spv;
	public AudioClip[] audioclip;

	[ HideInInspector]
	public AudioSource asc;
	// Use this for initialization
	void Start ()
	{

		if (spv != null && spv != this) {

			Destroy (this.gameObject);

		} else if (spv == null) {

			spv = this;

		}

		DontDestroyOnLoad (this);

		asc = GetComponent < AudioSource > ();
	}

	public void PlayAudioClip (AudioClip audioclip)
	{
		spv.asc.PlayOneShot (audioclip);
//		spv.asc.clip = audioclip;
//		spv.asc.Play ();
	}

	public void StopAudioClip ()
	{
		spv.asc.Pause ();
	}

	public void UnpauseAudioClip ()
	{
		spv.asc.UnPause ();
	}
}
