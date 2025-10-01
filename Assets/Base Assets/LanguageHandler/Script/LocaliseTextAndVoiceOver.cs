using UnityEngine;
using UnityEngine.UI;
using SmartLocalization;


//For Arabic only
using ArabicSupport;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif


public enum LocalizeType
{
	Only_Text,
	Only_Voice,
	TextAndVoice,
	Only_Font}
;

public class LocaliseTextAndVoiceOver : MonoBehaviour
{
	public LocalizeType localizeType;
	public bool StopVOOnDisable;
	public bool DoNotAlign;
	bool IsFirstTime = true;
	public bool DoNotChangeFont = false;
    #region ForFontName
    string fontName;
    #endregion
    //For Arabic only
    [Multiline]
	private string Text = "";
	private UnityEngine.UI.Text txt;
	static string[] AbsoluteVOKeys = { "completed _simulation.", "Title" };
	void CallLanguageChangeListener ()
	{
        if (GetComponentInChildren < TextMesh > () == null) {
			OnLanguageChangeListener ();
		} else {			
			OnLanguageChangeListener_textMesh ();
		}
	}

	void Start ()
	{
		//New line of code

		textComponent = GetComponentInChildren<Text> ();
		//=========================
        if (localizeType == LocalizeType.Only_Text) {
			CallLanguageChangeListener ();
			IsFirstTime = false;
		}

		if (localizeType == LocalizeType.TextAndVoice) {
			CallLanguageChangeListener ();
			PlayVoiceOver ();
			IsFirstTime = false;
		}

		if (localizeType == LocalizeType.Only_Voice) {
			PlayVoiceOver ();
			IsFirstTime = false;
		}

		if (localizeType == LocalizeType.Only_Font) {
			ChangeFont ();

			IsFirstTime = false;
		}
	}
    
	void OnEnable ()
	{
        // to Get the font  name
        GetFontNameFromAssetBundleManager();
        if (localizeType == LocalizeType.Only_Text || localizeType == LocalizeType.TextAndVoice)
        {
            LanguageHandler.LanguageChangeEventFire += GetFontNameFromAssetBundleManager;
            if (GetComponentInChildren < TextMesh > () == null) {				
				LanguageHandler.LanguageChangeEventFire += OnLanguageChangeListener;
			} else {
				LanguageHandler.LanguageChangeEventFire += OnLanguageChangeListener_textMesh;			
			}
		}

		if (!IsFirstTime) {
			if (localizeType == LocalizeType.Only_Text) {
				CallLanguageChangeListener ();
			}

			if (localizeType == LocalizeType.TextAndVoice) {
				CallLanguageChangeListener ();
				PlayVoiceOver ();
			}

			if (localizeType == LocalizeType.Only_Voice) {
				PlayVoiceOver ();
			}
			if (localizeType == LocalizeType.Only_Font) {              
				ChangeFont ();

			}
		}
	}
    void GetFontNameFromAssetBundleManager()
    {
        fontName = AssetBundleManager.Instance.GetFontName();
    }
	void OnDisable ()
	{		
		if (localizeType == LocalizeType.Only_Text || localizeType == LocalizeType.TextAndVoice)
        {
            LanguageHandler.LanguageChangeEventFire -= GetFontNameFromAssetBundleManager;
            if (GetComponentInChildren < TextMesh > () == null) {
				LanguageHandler.LanguageChangeEventFire -= OnLanguageChangeListener;
			} else {				
				LanguageHandler.LanguageChangeEventFire -= OnLanguageChangeListener_textMesh;
			}
		}

		if (StopVOOnDisable) {
			if ((localizeType == LocalizeType.Only_Voice || localizeType == LocalizeType.TextAndVoice)) {
				StopVoiceOver ();
			}
		}
	}

	public void OnLanguageChangeListener ()
	{
		if (LanguageHandler.instance.Languages [LanguageHandler.instance.CurrentLanguageIndex]._Alignment == LanguageData.LanguageTextAlignment.LeftToRight)
        {
			if (!DoNotAlign)
            {
				switch (GetComponentInChildren<Text> ().alignment)
                {
				case TextAnchor.UpperRight:
					GetComponentInChildren<Text> ().alignment = TextAnchor.UpperLeft;
					break;
				case TextAnchor.MiddleRight:
					GetComponentInChildren<Text> ().alignment = TextAnchor.MiddleLeft;
					break;
				case TextAnchor.LowerRight:
					GetComponentInChildren<Text> ().alignment = TextAnchor.LowerLeft;
					break;
				}
			}

			if (!DoNotChangeFont)
			{
                GetFontForText();
			}
			GetComponentInChildren < Text > ().text =  "" + LanguageManager.Instance.GetTextValue (gameObject.name);
		}
        else
        {
			if (!DoNotAlign) {
				switch (GetComponentInChildren<Text> ().alignment) {
				case TextAnchor.UpperLeft:
					GetComponentInChildren<Text> ().alignment = TextAnchor.UpperRight;
					break;
				case TextAnchor.MiddleLeft:
					GetComponentInChildren<Text> ().alignment = TextAnchor.MiddleRight;
					break;
				case TextAnchor.LowerLeft:
					GetComponentInChildren<Text> ().alignment = TextAnchor.LowerRight;
					break;
				}
			} 

			if (!DoNotChangeFont)
			{
                GetFontForText();
			}

			//GetComponentInChildren<Text> ().text = ArabicFixer.Fix ("" + LanguageManager.Instance.GetTextValue (gameObject.name), false, false); 
		//	ArabicLineFixer.instance.ArabicText = "" + LanguageManager.Instance.GetTextValue(gameObject.name);
		//	ArabicLineFixer.instance.textComponent = this.GetComponentInChildren<Text>();
		//	StartCoroutine(ArabicLineFixer.instance.FixLineOrderCoroutine());

		//	GetComponentInChildren<Text> ().gameObject.AddComponent<ArabicLineFixer> ();

//			if (GetComponentInChildren<Text> ().gameObject.GetComponent<ArabicLineFixer> () == null)
//			{
//
//				if(gameObject.GetComponent<RandomExclamation> () == null)
//					GetComponentInChildren<Text> ().gameObject.AddComponent<ArabicLineFixer> ();
//				else
//					GetComponentInChildren<Text> ().text = ArabicFixer.Fix ("" + LanguageManager.Instance.GetTextValue (gameObject.name), false, false);
//
//			}
			StartCoroutine (FixLineOrderCoroutine ("" + LanguageManager.Instance.GetTextValue (gameObject.name)));
//			else if (GetComponent<Text> ().gameObject.GetComponent<ArabicLineFixer> () == null )
//
//			{
//				if(gameObject.GetComponent<RandomExclamation> () == null)
//					GetComponent<Text> ().gameObject.AddComponent<ArabicLineFixer> ();
//				else
//					GetComponentInChildren<Text> ().text = ArabicFixer.Fix ("" + LanguageManager.Instance.GetTextValue (gameObject.name), false, false);
//
//			}

		}

	}

	public void OnLanguageChangeListener_textMesh ()
	{
        if (LanguageHandler.instance.Languages [LanguageHandler.instance.CurrentLanguageIndex]._Alignment == LanguageData.LanguageTextAlignment.LeftToRight)
        {
			if (GetComponentInChildren < TextMesh > ().alignment != TextAlignment.Center && !DoNotAlign) {				
				GetComponentInChildren < TextMesh > ().alignment = TextAlignment.Left;
			}

			if (!DoNotChangeFont)
            {
                GetFontForTextMesh();
            }

			GetComponentInChildren<TextMesh> ().text = "" + LanguageManager.Instance.GetTextValue (gameObject.name);

		}
        else
        {
			if (GetComponentInChildren < TextMesh > ().alignment != TextAlignment.Center && !DoNotAlign)
            {			
				GetComponentInChildren < TextMesh > ().alignment = TextAlignment.Right;			
			}

			if (!DoNotChangeFont)
            {
                GetFontForTextMesh();
			}

			GetComponentInChildren < TextMesh > ().text = ArabicFixer.Fix ("" + LanguageManager.Instance.GetTextValue (gameObject.name), false, false);		
		}
	}

	AudioClip _Clip;
	public void PlayVoiceOver()
	{
		if (GlobalAudioSrc.Instance == null)
			return;

		GlobalAudioSrc.Instance.audioSrc.clip = null;
		if ( PlayerPrefs.GetString("currentLanguage") == LanguageHandler.defaultLanguage) 
		{
			//Debug.Log("Audio file path of Rest"+PlayerPrefs.GetString("currentLanguage"));
			_Clip = Resources.Load < AudioClip >("VoiceOvers/" + LanguageHandler.instance.Languages[LanguageHandler.instance.CurrentLanguageIndex].LanguageID + "/" + gameObject.name);

		}
		else 
		{ 
			_Clip = AssetBundleManager.Instance.GetAssetBundle(PlayerPrefs.GetString("currentLanguage")).LoadAsset<AudioClip>(gameObject.name);

			if (_Clip == null)
			{
				return;
			}    
		}

		GlobalAudioSrc.Instance.audioSrc.clip = _Clip;

		GlobalAudioSrc.Instance.audioSrc.PlayOneShot(_Clip);
	}

	public void PlayVoiceOverForGaze ()
	{
		if (GlobalAudioSrc.Instance == null)
			return;
        
		if (IsPlayingAbsoluteVoice ())
			return;
        
		PlayVoiceOver ();
	}

	public void StopVoiceOver ()
	{
		if (GlobalAudioSrc.Instance == null)
			return;

		if (IsPlayingAbsoluteVoice ())
			return;
		GlobalAudioSrc.Instance.audioSrc.Stop ();
	}

	public bool IsPlayingAbsoluteVoice ()
	{
		if (GlobalAudioSrc.Instance.audioSrc.clip != null && GlobalAudioSrc.Instance.audioSrc.isPlaying) {
			for (int i = 0; i < AbsoluteVOKeys.Length; i++) {
				if (GlobalAudioSrc.Instance.audioSrc.clip.name.Equals (AbsoluteVOKeys [i]))
					return true;
			}
			return false;
		} else
			return false;
	}

	void ChangeFont ()
	{
		if (LanguageHandler.instance.Languages [LanguageHandler.instance.CurrentLanguageIndex]._Alignment == LanguageData.LanguageTextAlignment.LeftToRight) {
			if (GetComponentInChildren < TextMesh > () == null)
            {
                GetFontForText();
			}
            else
            {
                GetFontForTextMesh();
            }
		}

        else
        {
			if (GetComponentInChildren < TextMesh > () == null)
            {
                GetFontForText();
            }
            else
            {
                GetFontForTextMesh();
            }
		}
	}
    #region GetFontForTextAndTextMesh
    void GetFontForText()
    {
        if (PlayerPrefs.GetString("currentLanguage") != LanguageHandler.defaultLanguage)
        {
            GetComponentInChildren<Text>().font = AssetBundleManager.Instance.GetAssetBundle(PlayerPrefs.GetString("currentLanguage")).
                                                  LoadAsset<Font>(fontName);
        }
        else
        {
            GetComponentInChildren<Text>().font = Resources.Load<Font>("Fonts/" + fontName);
        }
    }

    void GetFontForTextMesh()
    {
        if (PlayerPrefs.GetString("currentLanguage") != LanguageHandler.defaultLanguage)
        {
            GetComponentInChildren<TextMesh>().font = AssetBundleManager.Instance.GetAssetBundle(PlayerPrefs.GetString("currentLanguage")).LoadAsset<Font>
                                                       (fontName);
            GetComponentInChildren<MeshRenderer>().material = AssetBundleManager.Instance.GetAssetBundle(PlayerPrefs.GetString("currentLanguage")).LoadAsset<Font>
                                                               (fontName).material;
        }
        else
        {
            GetComponentInChildren<TextMesh>().font = Resources.Load<Font>("Fonts/" + fontName);
            GetComponentInChildren<MeshRenderer>().material = Resources.Load<Font>("Fonts/" + fontName).material;
        }
    }
    #endregion
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
			Debug.Log("panel width distance "+ rt.rect.width);
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