using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using GrowlibLauncher;
using dpn;
using UnityEngine.UI;

public enum Platform
{
    GROWLIB,
Umety}
;

public class VrSelector : MonoBehaviour
{
    public static VrSelector instance;
    Canvas[] _canvas;
    public Platform platform;
    public bool mode;

    public VrSelector()
    {
        if(instance == null)
            instance = this;
    }

    public void Awake()
    {

        if (instance == this)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);

#if !UNITY_EDITOR
        mode = PlayerPrefs.GetInt ( "mode" ) != 0 ? true : false ;
#endif

		if (mode) {
			Dpvr.DpvrDeviceManager.SystemKeyManager.BlockHomeKey ();
		}
		else
		{
			Dpvr.DpvrDeviceManager.SystemKeyManager.AllowHomeKey ();
		}
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            // Either one command mode is ON(PICO or Umety)
            if ( mode)
            {
                //do nothing
            }
            else
            {
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    #if !UNITY_EDITOR
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    #endif
                    Application.Quit();
                    Debug.Log("Quit");
                }
            }
        }
    }

    public void EnablePhysicsRayCasters()
    {
        PhysicsRaycaster[] _dpr = FindObjectsOfType<PhysicsRaycaster>();

        for (int i = 0; i < _dpr.Length; i++)
        {
            _dpr[i].enabled = true;
        }
    }

    public void DisablePhysicsRayCasters()
    {
        PhysicsRaycaster[] _dpr = FindObjectsOfType<PhysicsRaycaster>();

        for (int i = 0; i < _dpr.Length; i++)
        {
            _dpr[i].enabled = false;
        }
    }

    public void EnableUIRayCasters()
    {
        _canvas = FindObjectsOfType<Canvas>();

        for (int i = 0; i < _canvas.Length; i++)
        {
            if (_canvas[i].GetComponent<GraphicRaycaster>() != null)
                _canvas[i].GetComponent<GraphicRaycaster>().enabled = true;

            if (_canvas[i].GetComponent<CurvedUI.CurvedUIRaycaster>() != null)
                _canvas[i].GetComponent<CurvedUI.CurvedUIRaycaster>().enabled = true;
        }

    }

    public void DisableUIRayCasters()
    {
        _canvas = FindObjectsOfType<Canvas>();

        for (int i = 0; i < _canvas.Length; i++)
        {
            if (_canvas[i].GetComponent<GraphicRaycaster>() != null)
                _canvas[i].GetComponent<GraphicRaycaster>().enabled = false;

            if (_canvas[i].GetComponent<CurvedUI.CurvedUIRaycaster>() != null)
                _canvas[i].GetComponent<CurvedUI.CurvedUIRaycaster>().enabled = false;
        }
    }
}
