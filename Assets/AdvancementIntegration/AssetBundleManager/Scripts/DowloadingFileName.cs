using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class DowloadingFileName : MonoBehaviour {

    
    void OnEnable()
    {
        for ( int i=0;i<LanguageHandler.instance.Languages.Count;i++)
        {
            if( PlayerPrefs.GetString("currentLanguage") ==  LanguageHandler.instance.Languages[i].LanguageID )
            {
				this.gameObject.GetComponent<Text>().text = AssetBundleManager.Instance.GetTopicID().ToLower()+"_"+LanguageHandler.instance.Languages[i].DisplayName;        
            }
        }           
    }
}
