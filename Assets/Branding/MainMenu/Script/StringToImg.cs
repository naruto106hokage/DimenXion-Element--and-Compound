using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UmetyDatabase;
public class StringToImg : MonoBehaviour
{

    public Image imgLogo;


    // Use this for initialization
    void Start()
    {
        string strc =  DatabaseManager.dbm.logo;
       
        if(strc == "Default")
        {
            imgLogo.transform.parent.gameObject.SetActive(true);
            Debug.Log("Default");
            return;
        }
        else if(strc == "None" || strc == "")
        {
            #if UNITY_EDITOR
			imgLogo.transform.parent.gameObject.SetActive(true);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			imgLogo.transform.parent.gameObject.SetActive(false);
            #endif
            
        }
        else
        {
             Debug.Log("else");
            byte[] imageBytes = Convert.FromBase64String(DatabaseManager.dbm.logo);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);
            Sprite _sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            //Vector2 size = _sprite.bounds.size;
            //imgLogo.GetComponent<RectTransform>().sizeDelta=()
            imgLogo.sprite = _sprite;
            imgLogo.transform.parent.gameObject.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
