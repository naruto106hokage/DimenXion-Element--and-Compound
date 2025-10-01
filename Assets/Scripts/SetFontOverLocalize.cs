using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetFontOverLocalize : MonoBehaviour
{
    public Font mrFont;
    void Start()
    {
       if (PlayerPrefs.GetString("currentLanguage") == "mr-IN"|| PlayerPrefs.GetString("currentLanguage") == "hi-IN")
        {
        if(GetComponent<LocaliseTextAndVoiceOver>()!=null){
            GetComponent<LocaliseTextAndVoiceOver>().enabled=false;
        }else{
            if(GetComponentInParent<LocaliseTextAndVoiceOver>()!=null){
                GetComponentInParent<LocaliseTextAndVoiceOver>().enabled=false;
            }
        }
            GetComponent<Text>().font=mrFont;
       }
    }

}
