using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using dpn;

public class PlayPauseSimulation : MonoBehaviour
{
    public static PlayPauseSimulation instance;

    public GameObject playPauseSimulationCanvas;
    public GameObject startMessageCanvas;

    [HideInInspector]
    public bool isPaused = false;

    [HideInInspector]
    public bool isWaitingForPause = false;

    AndroidJavaClass jc;

    //	[ HideInInspector]
    public AudioSource[ ] audioScript;

    //	[ HideInInspector]
    public List <bool> audioPlaying;

    bool simualtionAlreadyPaused;

    void Awake()
    {
        audioScript = Resources.FindObjectsOfTypeAll < AudioSource >();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);	
        }
        else
        {
            Destroy(gameObject);
        }
						
        audioPlaying.Clear();

        #if !UNITY_EDITOR && UNITY_ANDROID

        jc = new AndroidJavaClass("broadcastplugin.umety.com.masterappbroadcast.DPlay");
        jc.CallStatic("createInstance", this.name); 

        jc = new AndroidJavaClass("broadcastplugin.umety.com.masterappbroadcast.DPause");
        jc.CallStatic("createInstance", this.name); 

        jc = new AndroidJavaClass("broadcastplugin.umety.com.masterappbroadcast.DStop");
        jc.CallStatic("createInstance", this.name);

        #endif
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "LoadingScene")
        {
            if (isWaitingForPause)
                OnPause();
        }
    }

    public void ShowStartErrorMessage(string messageToBeShown)
    {
        startMessageCanvas.GetComponentInChildren < Text >().text = messageToBeShown;

		startMessageCanvas.transform.SetParent(DpnCameraRig._instance._center_eye.transform);

		startMessageCanvas.transform.localPosition = new Vector3(0f, 0f, ((1 / DpnCameraRig._instance._center_eye.transform.parent.localScale.z) * 0.6f));
        startMessageCanvas.transform.localRotation = Quaternion.identity;
        startMessageCanvas.SetActive(true);

        AudioListener.volume = 0;

        Time.timeScale = 0;
    }


    public void OnPlay()
    {
        if (SceneManager.GetActiveScene().name != "LoadingScene")
        {
			for (int i = 0; i < DpnCameraRig._instance.transform.parent.childCount; i++)
            {
				if (DpnCameraRig._instance.transform.parent.GetChild(i).GetComponent<Camera>())
                {
					DpnCameraRig._instance.transform.parent.GetChild(i).gameObject.SetActive(true);
                }
            }
            SetTimeScale(1);
        }
    }

    public void OnPause()
    {
        if (SceneManager.GetActiveScene().name != "LoadingScene")
        {
			for (int i = 0; i < DpnCameraRig._instance.transform.parent.childCount; i++)
            {
                if (DpnCameraRig._instance.transform.parent.GetChild(i).GetComponent<Camera>())
                {
                    DpnCameraRig._instance.transform.parent.GetChild(i).gameObject.SetActive(false);
                }
            }
            isWaitingForPause = false;
            SetTimeScale(0);
        }
        else
        {
            isWaitingForPause = true;
        }
    }

    public void OnStop()
    {
        System.Diagnostics.Process.GetCurrentProcess().Kill();
        Application.Quit();
    }

    public void SetTimeScale(float scale)
    {
        if (scale == 1)
        {
            playPauseSimulationCanvas.SetActive(false);
            playPauseSimulationCanvas.transform.SetParent(transform);

            HandleAudio("Pause");
            AudioListener.volume = 1;
            isPaused = false;

            if (simualtionAlreadyPaused)
            {
                scale = 0;
            }

        }
        else if (scale == 0)
        {
            playPauseSimulationCanvas.transform.SetParent(DpnCameraRig._instance._center_eye.transform);
            playPauseSimulationCanvas.transform.localPosition = new Vector3(0f, 0f, ((1 / DpnCameraRig._instance._center_eye.transform.parent.localScale.z) * 0.6f));
            playPauseSimulationCanvas.transform.localRotation = Quaternion.identity;
            playPauseSimulationCanvas.SetActive(true);

            HandleAudio("Play");
            AudioListener.volume = 0;

            isPaused = true;

            if (Time.timeScale == 0)
            {

                simualtionAlreadyPaused = true;

            }
            else
            {

                simualtionAlreadyPaused = false;
			
            }

        }

        Time.timeScale = scale;

    }

    public void OnPauseWeb()
    {
        OnPause();
    }

    public void OnPlayWeb(float time)
    {
        OnPlay();
    }

    public void HandleAudio(string state)
    {

        audioScript = Resources.FindObjectsOfTypeAll < AudioSource >();

        if (audioScript.Length == 0)
            return;

        if (state == "Pause")
        {

            if (audioPlaying.Count == 0)
                return;

            for (int i = 0; i < audioScript.Length; i++)
            {

                if (audioPlaying[i])
                {

                    audioScript[i].UnPause();

                }

            }


        }
        else if (state == "Play")
        {

            audioPlaying.Clear();

            for (int i = 0; i < audioScript.Length; i++)
            {


                if (audioScript[i].isPlaying)
                {

                    audioPlaying.Add(true);
                    audioScript[i].Pause();

                }
                else
                    audioPlaying.Add(false);

            }
		
        }

    }

}



