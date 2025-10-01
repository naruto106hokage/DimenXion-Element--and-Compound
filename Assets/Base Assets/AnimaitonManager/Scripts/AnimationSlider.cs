using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimationSlider : MonoBehaviour
{
    AudioSource specAnalyzerAudiosource;
    public SpectrumAnalyzer specAnalyzer;

    public Animation _anim;
    AnimationState animState;
    public string animationName = "Take 001";
    public AudioSource animationAudio;

    public Slider slider;
    public GameObject PlayPauseButton;

    public Sprite playSprite;
    public Sprite pauseSprite;

    public Text durationText;

    public float animationDuration;
    public  int animationDurationInSeconds;
    public float animationDurationInMinutes;

    public bool isPointerDown;
    public GameObject skipbutton;
    bool isPlaying;

    void Start()
    {
        Refresh();
        specAnalyzerAudiosource = specAnalyzer.GetComponent<AudioSource>();
    }


    public void Refresh()
    {
        isPlaying = true;
        animState = _anim[animationName];
        slider.maxValue = animState.length;

        animationDuration = animState.clip.length;

        while (animationDuration >= 60)
        {
            animationDurationInMinutes += 1;
            animationDuration -= 60;
        }
        animationDurationInSeconds = (int)animationDuration;
    }



    bool globalAudioEnabled = false;

    void Update()
    {
        slider.value = animState.time;
//		Debug.Log (animState.time);
//		if(slider.value == 1.7917)
			

        if (animationDurationInSeconds >= 10)
            durationText.text = ((int)(animState.time) / 60).ToString() + ":" + (((int)animState.time) % 60).ToString("D2") + "/" + animationDurationInMinutes + ":" + animationDurationInSeconds;
        else
            durationText.text = ((int)(animState.time) / 60).ToString() + ":" + (((int)animState.time) % 60).ToString("D2") + "/" + animationDurationInMinutes + ":0" + animationDurationInSeconds;


        if (!_anim.isPlaying && !globalAudioEnabled)
        {
            // HANDLE AFTER ANIMATION PHASE HERE
            GlobalAudioSrc.Instance.audioSrc.enabled = true;
            globalAudioEnabled = true;
        }
    }

    public void SliderDown()
    {
        isPointerDown = true;
        animState.speed = 0;
        animationAudio.Pause();
    }

    public void SliderUp()
    {
        isPointerDown = false;
        if (isPlaying)
        {
            animState.speed = 1;
            animationAudio.UnPause();

        }

//        animationAudio.time = Mathf.Clamp(slider.value, 0f, animationAudio.clip.length);
        specAnalyzerAudiosource.time = Mathf.Clamp(slider.value, 0f, specAnalyzerAudiosource.clip.length);
        specAnalyzer.LinesPassed();
    }

    public void OnSliderChange()
    {
        animState.time = slider.value;
        //animationAudio.time = slider.value;
    }

    public void EnableSliderImage(Image imagetoBeEnabled)
    {
        imagetoBeEnabled.enabled = true;
    }

    public void DisableSliderImage(Image imagetoBeDisabled)
    {
        if (!isPointerDown)
            imagetoBeDisabled.enabled = false;
    }

    public void PlayPauseToggle()
    {
        SoundManager.instance.PlayClickSound();
        if (isPlaying)
        {
            specAnalyzerAudiosource.Pause();
            SpectrumAnalyzer.instance.animationPlaying = false;
            animState.speed = 0f;
            animationAudio.Pause();	
            PlayPauseButton.GetComponent<Image>().sprite = playSprite;
            isPlaying = false;
        }
        else
        {
            specAnalyzerAudiosource.UnPause();
            SpectrumAnalyzer.instance.animationPlaying = true;
            animState.speed = 1f;
            animationAudio.UnPause();
            PlayPauseButton.GetComponent<Image>().sprite = pauseSprite;
            isPlaying = true;
        }
    }

    public void Go2Scene(int BuildIndex)
    {
        SoundManager.instance.PlayClickSound();
        LoadingScene.LoadingSceneIndex = BuildIndex;
    }

    #region modifiedByHarsh

    public void OnSkipButton()
    {
        SoundManager.instance.PlayClickSound();
    }

    #endregion
}
