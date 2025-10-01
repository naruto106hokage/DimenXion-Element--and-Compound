using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTitle : MonoBehaviour {

	// Use this for initialization
    void OnEnable()
    {
        for ( int i=0;i<LanguageHandler.instance.Languages.Count;i++)
        {
            if( PlayerPrefs.GetString("currentLanguage") ==  LanguageHandler.instance.Languages[i].LanguageID )
            {
                this.gameObject.GetComponent<Text>().text = LanguageHandler.instance.Languages[i].DisplayName;        
            }
        }
       // Debug.Log("DiaplayName"+this.gameObject.GetComponent<Text>().text);           
    }
}
