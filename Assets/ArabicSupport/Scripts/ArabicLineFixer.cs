using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArabicSupport;
using System.Linq;
using SmartLocalization;

[RequireComponent(typeof(Text))]
[ExecuteInEditMode]
public class ArabicLineFixer : MonoBehaviour
{
    public static ArabicLineFixer instance;

	public delegate void Language_Arabic ();

	public static event Language_Arabic LanguageArabicEventFire;

   
    [TextArea]
	internal string ArabicText;
	public Text textComponent;


	string keyName;
    private void Awake()
    {
      

			instance = this;



		if (gameObject.transform.parent.gameObject.GetComponent<LocaliseTextAndVoiceOver> () != null) 
		{
			keyName = gameObject.transform.parent.name;
		}

		else if (gameObject.GetComponent<LocaliseTextAndVoiceOver> () != null) 
		{
			keyName = gameObject.name;

		}
	



		ArabicText = "" + LanguageManager.Instance.GetTextValue(keyName);
		textComponent = GetComponent<Text>();
		FixArabicText ();
	
			
    }

	void OnEnable(){
		LanguageArabicEventFire += FixArabicText;
	}
	void OnDisable(){
		LanguageArabicEventFire -= FixArabicText;
	}


	void FixArabicText(){

		if (LanguageHandler.instance.Languages [LanguageHandler.instance.CurrentLanguageIndex]._Alignment == LanguageData.LanguageTextAlignment.RightToLeft) {
			StartCoroutine (FixLineOrderCoroutine ());
		}
	}

    public void SetArabicText(string text)
    {
        this.ArabicText = text;
        StartCoroutine(FixLineOrderCoroutine());
    }

    void OnValidate()
    {
        StartCoroutine(FixLineOrderCoroutine());
    }

   public IEnumerator FixLineOrderCoroutine()
    {
        List<string> resultText = new List<string>();
        RectTransform rt = textComponent.GetComponent<RectTransform>();
		Debug.Log ("AP1000 " +"0");

        List<string> paragraphList = ArabicText.Split('\n').ToList();
		Debug.Log ("AP1000 " +"1");
        foreach (string paragraph in paragraphList)
        {
			Debug.Log ("AP1000 " +"2");

            textComponent.text = ArabicFixer.Fix(paragraph, false, false);
            TextGenerationSettings tgs = textComponent.GetGenerationSettings(rt.rect.size);

            if (textComponent.text.IndexOf(' ') < 0)
            {
                resultText.Add(textComponent.text);
				Debug.Log ("AP1000 " +"3");

            }
            else
            {
                List<string> lineList = new List<string>();
                List<string> wordList = textComponent.text.Split(' ').ToList();
                string singleLine = "";
				Debug.Log ("AP1000 " +"4");

                while (wordList.Count > 0)
                {
                    string singleWord = wordList[wordList.Count - 1];
                    wordList.RemoveAt(wordList.Count - 1);
					Debug.Log ("AP1000 " +"5");

                    if (textComponent.cachedTextGenerator.GetPreferredWidth(singleWord + ' ' + singleLine, tgs) > rt.rect.width)
                    {
                        lineList.Add(singleLine);
                        singleLine = singleWord;
						Debug.Log ("AP1000 " +"6");

                    }
                    else
                    {
                        singleLine = (singleLine != "") ? singleWord + ' ' + singleLine : singleWord;
						Debug.Log ("AP1000 " +"7");

                    }
                }

                if (singleLine.Length > 0)
                    lineList.Add(singleLine);

				Debug.Log ("AP1000 " +"8");


                resultText.Add(String.Join(Environment.NewLine, lineList.ToArray()));
            }

            if (!Application.isEditor)
                yield return new WaitForEndOfFrame();

			Debug.Log ("AP1000 " +"9");

        }
        textComponent.text = String.Join(Environment.NewLine, resultText.ToArray());
		Debug.Log ("AP1000 " +"10");

    }
}