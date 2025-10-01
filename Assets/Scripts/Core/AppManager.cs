using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using InfinityEngine.Localization;

public class AppManager : MonoBehaviour
{

	public static AppManager instance;


	public GameObject speechEnginePrefab;

	void Awake ()
	{
		instance = this;
        if (GameObject.FindObjectOfType<SpeechRecognizationManager>() == null)
            Instantiate(speechEnginePrefab);
    }

	



}
