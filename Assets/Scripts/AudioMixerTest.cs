using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerTest : MonoBehaviour {

	public AudioMixer am;

	public AudioMixerGroup bg_music, microphone_audio; 

	void Awake () {

		if (dpn.DpnCameraRig._instance._center_eye.GetComponent<AudioSource> () == null)
			return;

        dpn.DpnCameraRig._instance._center_eye.GetComponent<AudioSource> ().outputAudioMixerGroup = bg_music;
        dpn.DpnCameraRig._instance._center_eye.GetComponent<AudioSource> ().priority = 255;

		if (dpn.DpnCameraRig._instance._center_eye.transform.parent.gameObject.AddComponent<AudioSource> () == null) {
            dpn.DpnCameraRig._instance._center_eye.transform.parent.gameObject.AddComponent<AudioSource> ().outputAudioMixerGroup = microphone_audio;
            dpn.DpnCameraRig._instance._center_eye.transform.parent.gameObject.AddComponent<AudioSource> ().priority = 0;
		} else {
            dpn.DpnCameraRig._instance._center_eye.transform.parent.gameObject.GetComponent<AudioSource> ().outputAudioMixerGroup = microphone_audio;
            dpn.DpnCameraRig._instance._center_eye.transform.parent.gameObject.GetComponent<AudioSource> ().priority = 0;
		}
	}

}
