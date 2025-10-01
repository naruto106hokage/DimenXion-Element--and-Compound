using UnityEngine;
using System.Collections;

/// <summary>
/// Common SFX Sounds
/// </summary>
public class SoundManager : MonoBehaviour
{
	
	public static SoundManager instance;

	public AudioClip ClickSound;
	public AudioClip ScrollSound;
	public AudioClip RightSound;
	public AudioClip WrongSound;


	[HideInInspector]
	public AudioSource asc;

	void Start ()
	{
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else
			Destroy (gameObject);
        
		asc = GetComponent<AudioSource> ();
	}

	public void PlayClickSound ()
	{
		instance.asc.PlayOneShot (ClickSound);
	}

	public void PlayScrollSound ()
	{
		instance.asc.PlayOneShot (ScrollSound);
	}

	public void PlayRightSound ()
	{
		instance.asc.PlayOneShot (RightSound);
	}

	public void PlayWrongSound ()
	{
		instance.asc.PlayOneShot (WrongSound);
	}

	public void PlaySound (AudioClip audioClip)
	{
		instance.asc.PlayOneShot (audioClip);
	}

	public void PlayAudioClip (AudioClip audioClip)
	{
		instance.asc.PlayOneShot (audioClip);

	}
}
