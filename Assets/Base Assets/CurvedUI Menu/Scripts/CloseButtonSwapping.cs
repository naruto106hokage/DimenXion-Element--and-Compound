using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UmetyDatabase;

public class CloseButtonSwapping: MonoBehaviour
{
    public static CloseButtonSwapping Instance;
    public GameObject crossButton;
    public GameObject languageButton;

    public bool mode;
    private Vector3 crossButtonPosition;

    Vector3 _intialPosCrossBtn;
    Vector3 _intialPosLanguage;

    void Awake()
    {
        Instance = this;
        _intialPosCrossBtn = crossButton.transform.position;
        _intialPosLanguage = languageButton.transform.position;
    }

    void Start()
    {
       
    }

    public void Swap_Btns()
    {
        if (LanguageHandler.instance.IsLeftToRight)
        {			
            crossButton.transform.position = _intialPosCrossBtn;
            languageButton.transform.position = _intialPosLanguage;
        }
        else
        {
            crossButton.transform.position = new Vector3(-1* _intialPosCrossBtn.x, _intialPosCrossBtn.y, _intialPosCrossBtn.z);
            languageButton.transform.position = new Vector3(-1 * _intialPosLanguage.x, _intialPosLanguage.y, _intialPosLanguage.z);
        }

        #if !UNITY_EDITOR
        mode = PlayerPrefs.GetInt ( "mode" ) != 0 ? true : false ;
        #endif

        if (mode)
        {
			PlayerPrefs.SetString ("QuitMode","NonQuittable");
            crossButton.SetActive(false);
        }
        else
		{
			PlayerPrefs.SetString ("QuitMode","Quittable");
            crossButton.SetActive(true);
        }
    }
}