using UnityEngine;
using System.Collections;
using SmartLocalization;
using UnityEngine.UI;
using ArabicSupport;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
[System.Serializable]
public class LanguageData
{
    public enum LanguageTextAlignment
    {
        LeftToRight,
        RightToLeft
    }

    ;

    //Display Name Shows it's native name;
    //LanguageId shows Culture code
    public string DisplayName, LanguageID;
    public LanguageTextAlignment _Alignment;
    // Make this constructor to add langs description by  Editor AdvancementIntegrationEditor Line 108
    public LanguageData(string DisplayName, string LanguageID, LanguageTextAlignment _Alignment)
    {
        this.DisplayName = DisplayName;
        this.LanguageID = LanguageID;
        this._Alignment = _Alignment;
    }
}

public class LanguageHandler : MonoBehaviour
{
    private static LanguageHandler Instance = null;

    public static LanguageHandler instance
    {
        get
        {
            if (Instance == null)
                Instance = FindObjectOfType<LanguageHandler>();
            return Instance;
        }

    }

    public delegate void LanguageChanged();

    public static event LanguageChanged LanguageChangeEventFire;

    [Header("This will set tool language editing bar ")]
    public List<LanguageData> Languages;
    [HideInInspector]
    public int CurrentLanguageIndex;

    public const int _DefaultLanguageIndex = 1;

    public bool IsLeftToRight
    {
        get
        {
            return (LanguageHandler.instance.Languages[LanguageHandler.instance.CurrentLanguageIndex]._Alignment == LanguageData.LanguageTextAlignment.LeftToRight);
        }
    }

    public bool IsRightToLeft
    {
        get
        {
            return (LanguageHandler.instance.Languages[LanguageHandler.instance.CurrentLanguageIndex]._Alignment == LanguageData.LanguageTextAlignment.RightToLeft);
        }
    }

    #region SetDefaultLang

    //if you change default language en-us to something different
    // you need to change degault lang in LanguageHandlerEditor
    //Line no 42
    public const string defaultLanguage = "en-US";
    //public const string defaultLanguage = "zh-CN";
    //public const string defaultLanguage = "ar-AR";

    #endregion

    internal LoadingScene loadingScene;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "LoadingScene")
        {
            loadingScene = FindObjectOfType<LoadingScene>();
            if (loadingScene == null)
            {
                Debug.LogError("LoadingCanvasIsMissingInLoadingScene");
            }
            else
                loadingScene.gameObject.SetActive(false);
        }

        // below condition is if your playing your app first time
        if (!PlayerPrefs.HasKey("currentLanguage"))
            PlayerPrefs.SetString("currentLanguage", "en-US");
        //Step1
        if (PlayerPrefs.GetString("currentLanguage") == defaultLanguage)
        {
            setCurrentLanguage();
            changeLanguageUpdate();
            if (loadingScene != null)
            {
                loadingScene.gameObject.SetActive(true);
            }

        }
        // below condition will see asset bundle is downloaded or not for other lang
        else
        {
            CheckAssetBundleForCurrentLanguage();
        }

    }

    public void CheckAssetBundleForCurrentLanguage()
    {
        //Asset bundle is on the path
        if (AssetBundleManager.Instance.IsFileExist())
        {
            setCurrentLanguage();
            changeLanguageUpdate();
            if (loadingScene != null)
            {
                loadingScene.gameObject.SetActive(true);
            }
        }
        //Asset bundle is not on the path
        else
        {
            InAbsenseOfAssetBundle.Instance.EnableAskForDownload();
        }
    }

    internal void setCurrentLanguage()
    {
        if (!PlayerPrefs.HasKey("currentLanguage"))
        {
            PlayerPrefs.SetString("currentLanguage", defaultLanguage);
        }
        else
        {
            for (int i = 0; i < Languages.Count; i++)
            {
                if (PlayerPrefs.GetString("currentLanguage").Equals(Languages[i].LanguageID))
                {
                    CurrentLanguageIndex = i;
                    return;
                }
            }
        }
    }

    internal void changeLanguageUpdate()
    {
        LanguageManager.Instance.ChangeLanguage(Languages[CurrentLanguageIndex].LanguageID);
        if (LanguageChangeEventFire != null)
            LanguageChangeEventFire();
    }

    public void OnLanguageChangeListener(Text TextBox, string CurrKey)
    {
        //New line of code

        textComponent = TextBox.GetComponent<Text>();
        //=========================
        if (LanguageHandler.instance.Languages[LanguageHandler.instance.CurrentLanguageIndex]._Alignment == LanguageData.LanguageTextAlignment.RightToLeft)
        {
            switch (TextBox.alignment)
            {
                case TextAnchor.UpperLeft:
                    TextBox.alignment = TextAnchor.UpperRight;
                    break;
                case TextAnchor.MiddleLeft:
                    TextBox.alignment = TextAnchor.MiddleRight;
                    break;
                case TextAnchor.LowerLeft:
                    TextBox.alignment = TextAnchor.LowerRight;
                    break;
            }
            //TextBox.text = ArabicFixer.Fix ("" + LanguageManager.Instance.GetTextValue (CurrKey));   
            ////New line of code


            StartCoroutine(FixLineOrderCoroutine("" + LanguageManager.Instance.GetTextValue(CurrKey)));


            //==========================
        }
        else
        {
            switch (TextBox.alignment)
            {
                case TextAnchor.UpperRight:
                    TextBox.alignment = TextAnchor.UpperLeft;
                    break;
                case TextAnchor.MiddleRight:
                    TextBox.alignment = TextAnchor.MiddleLeft;
                    break;
                case TextAnchor.LowerRight:
                    TextBox.alignment = TextAnchor.LowerLeft;
                    break;
            }
            TextBox.text = "" + LanguageManager.Instance.GetTextValue(CurrKey);
        }
    }

    public void OnLanguageChangeListener(TextMesh TextBox, string CurrKey)
    {
        if (LanguageHandler.instance.Languages[LanguageHandler.instance.CurrentLanguageIndex]._Alignment == LanguageData.LanguageTextAlignment.RightToLeft)
        {
            TextBox.alignment = TextAlignment.Right;

            TextBox.text = ArabicFixer.Fix("" + LanguageManager.Instance.GetTextValue(CurrKey));
        }
        else
        {
            TextBox.alignment = TextAlignment.Left;

            TextBox.text = "" + LanguageManager.Instance.GetTextValue(CurrKey);
        }
    }

    public void PlayVoiceOver(string key)
    {
        if (GlobalAudioSrc.Instance.audioSrc.isPlaying && GlobalAudioSrc.Instance.audioSrc.clip.name == key)
            return;

        GlobalAudioSrc.Instance.audioSrc.clip = null;


        AudioClip _Clip;
        if (PlayerPrefs.GetString("currentLanguage") == defaultLanguage)
        {
            _Clip = Resources.Load<AudioClip>("VoiceOvers/" + LanguageHandler.instance.Languages[LanguageHandler.instance.CurrentLanguageIndex].LanguageID + "/" + key);

        }
        else
        {
            _Clip = AssetBundleManager.Instance.GetAssetBundle(PlayerPrefs.GetString("currentLanguage")).LoadAsset<AudioClip>(key);

            if (_Clip == null)
            {
                return;
            }
        }

        GlobalAudioSrc.Instance.audioSrc.clip = _Clip;
        GlobalAudioSrc.Instance.audioSrc.PlayOneShot(_Clip);
    }

    public void StopVoiceOver()
    {
        if (GlobalAudioSrc.Instance == null)
            return;

        GlobalAudioSrc.Instance.audioSrc.Stop();
    }

    public void PlayBackMenuVoiceOver(string key)
    {
        if (GlobalAudioSrc.Instance.SecondAudioSrc.isPlaying && GlobalAudioSrc.Instance.SecondAudioSrc.clip.name == key)
            return;
        GlobalAudioSrc.Instance.SecondAudioSrc.clip = null;
        //    AudioClip _Clip = Resources.Load<AudioClip>("VoiceOvers/" + LanguageHandler.instance.Languages[LanguageHandler.instance.CurrentLanguageIndex].LanguageID + "/" + key);
        AudioClip _Clip;
        if (PlayerPrefs.GetString("currentLanguage") == defaultLanguage)
        {
            //Debug.Log("Audio file path of Rest"+PlayerPrefs.GetString("currentLanguage"));
            _Clip = Resources.Load<AudioClip>("VoiceOvers/" + LanguageHandler.instance.Languages[LanguageHandler.instance.CurrentLanguageIndex].LanguageID + "/" + key);

        }
        else
        {
            _Clip = AssetBundleManager.Instance.GetAssetBundle(PlayerPrefs.GetString("currentLanguage")).LoadAsset<AudioClip>(key);

            if (_Clip == null)
            {
                return;
            }
        }
        GlobalAudioSrc.Instance.SecondAudioSrc.clip = _Clip;
        GlobalAudioSrc.Instance.SecondAudioSrc.PlayOneShot(_Clip);
    }

    public void StopBackMenuVoiceOver()
    {
        if (GlobalAudioSrc.Instance == null)
            return;

        GlobalAudioSrc.Instance.SecondAudioSrc.Stop();
    }

    public bool CheckIfLanguageExist(string _currentLanguage)
    {
        List<SmartCultureInfo> languages = SmartLocalization.LanguageManager.Instance.GetSupportedLanguages();

        for (int i = 0; i < languages.Count; i++)
        {
            if (languages[i].languageCode == _currentLanguage)
            {
                return true;
            }
        }
        return false;
    }

    public AudioClip GetLength(string key)
    {

        if (PlayerPrefs.GetString("currentLanguage") == LanguageHandler.defaultLanguage)
        {
            // Debug.Log("Audio file path of Rest"+PlayerPrefs.GetString("currentLanguage"));
            if (Resources.Load<AudioClip>("VoiceOvers/" +
                LanguageHandler.instance.Languages[LanguageHandler.instance.CurrentLanguageIndex].LanguageID + "/" + key) == null)
            {
                Debug.LogError("Audio with this name is missing");
                return null;
            }

            return Resources.Load<AudioClip>("VoiceOvers/" +
            LanguageHandler.instance.Languages[LanguageHandler.instance.CurrentLanguageIndex].LanguageID + "/" + key);

        }
        else
        {
            if (AssetBundleManager.Instance.GetAssetBundle(PlayerPrefs.GetString("currentLanguage")).LoadAsset<AudioClip>(key) == null)
            {
                Debug.LogError("Audio with this name is missing");
                return null;
            }

            return AssetBundleManager.Instance.GetAssetBundle(PlayerPrefs.GetString("currentLanguage")).LoadAsset<AudioClip>(key);
        }

    }

    //new line of code===


    public Text textComponent;

    public IEnumerator FixLineOrderCoroutine(string ArabicText)
    {
        List<string> resultText = new List<string>();
        RectTransform rt = textComponent.GetComponent<RectTransform>();

        List<string> paragraphList = ArabicText.Split('\n').ToList();
        TextGenerator textGen = new TextGenerator();
        foreach (string paragraph in paragraphList)
        {
            Debug.Log("paragraph " + paragraph);
            textComponent.text = ArabicFixer.Fix(paragraph, false, false);
            TextGenerationSettings tgs = textComponent.GetGenerationSettings(rt.rect.size);
            Debug.Log("panel width distance " + rt.rect.width);
            if (textComponent.text.IndexOf(' ') < 0)
            {
                resultText.Add(textComponent.text);

            }
            else
            {
                List<string> lineList = new List<string>();
                List<string> wordList = textComponent.text.Split(' ').ToList();
                string singleLine = "";

                Debug.Log("wordList.Count " + wordList.Count);
                while (wordList.Count > 0)
                {
                    string singleWord = wordList[wordList.Count - 1];
                    wordList.RemoveAt(wordList.Count - 1);

                    //  Debug.Log("AP100  " + textComponent.cachedTextGenerator.GetPreferredWidth("abhishesh", tgs));
                    // Debug.Log("AP100  " + GetComponent<Text>().preferredWidth);
                    Debug.Log("AP100 ppppp " + textComponent.preferredWidth);
                    textComponent.text = singleWord + ' ' + singleLine;

                    // if (textComponent.cachedTextGenerator.GetPreferredWidth(singleWord + ' ' + singleLine, tgs) > rt.rect.width)
                    if (textComponent.preferredWidth > rt.rect.width)
                    {
                        lineList.Add(singleLine);
                        singleLine = singleWord;

                    }
                    else
                    {
                        singleLine = (singleLine != "") ? singleWord + ' ' + singleLine : singleWord;
                    }
                }

                if (singleLine.Length > 0)
                    lineList.Add(singleLine);



                resultText.Add(String.Join(Environment.NewLine, lineList.ToArray()));
            }

            if (!Application.isEditor)
                yield return new WaitForEndOfFrame();


        }
        textComponent.text = String.Join(Environment.NewLine, resultText.ToArray());

    }

    //===================

}
