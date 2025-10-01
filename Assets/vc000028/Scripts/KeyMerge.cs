using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SmartLocalization;
using System.Collections;
using System.Linq;
using System;
using ArabicSupport;

public class KeyMerge : MonoBehaviour
{
	public List<string> key;
	string mergerText;


	void OnEnable()
	{
		Invoke ("MergeText" , 0.01f);
	}

	void MergeText ()
	{

		/******<Start>************** Add all key's text in  mergerText string ************************/
		for (int i = 0; i < key.Count; i++) 
		{
			{
				if (i == 0)
					mergerText = LanguageManager.Instance.GetTextValue (key [i]);
				else 
					mergerText += " " + LanguageManager.Instance.GetTextValue (key [i]);
			}
		}
		/*******<End>************* Add all key's text in mergerText string **************************/
				

		if (LanguageHandler.instance.IsRightToLeft) 
		{
			GetComponentInChildren<Text> ().alignment = TextAnchor.MiddleRight;     // for arabic standard, change text anchor to right.
			//GetComponentInChildren<Text> ().text = ArabicSupport.ArabicFixer.Fix (mergerText, false, false);
//			ArabicLineFixer.instance.ArabicText = mergerText;
//			ArabicLineFixer.instance.textComponent = this.GetComponentInChildren<Text>();
//			StartCoroutine(ArabicLineFixer.instance.FixLineOrderCoroutine());


			//New line of code

			textComponent =GetComponentInChildren<Text> ();
			//=========================

			StartCoroutine (FixLineOrderCoroutine ("" + mergerText));

		} 
		else 
		{
			GetComponentInChildren<Text> ().alignment = TextAnchor.MiddleCenter;     // for other, change text anchor to center.
			GetComponentInChildren<Text> ().text = mergerText;
		}

		StartCoroutine (PlayVoiceOver (0));   // Start Playing VoiceOver sequentially as key added in key list.
	}



	/* Play Voice Over sequentially as key added in key list */
	IEnumerator PlayVoiceOver (int i)
	{
		do {
			LanguageHandler.instance.PlayVoiceOver (key [i]);

			if (GlobalAudioSrc.Instance.audioSrc.clip != null)
				yield return new WaitForSeconds (GlobalAudioSrc.Instance.audioSrc.clip.length);
			
			i++;

		} while(i < key.Count);
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
