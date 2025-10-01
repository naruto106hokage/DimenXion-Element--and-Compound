using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class fontSizeChangerForhiin : MonoBehaviour
{

	public int hiFontSize;
    public Vector3 HindiPos;
    // Use this for initialization
    void Start()
	{


		if (PlayerPrefs.GetString("currentLanguage") == "hi-IN")
		{	if(GetComponent<Text>())
			{
				GetComponent<Text>().resizeTextForBestFit = false;
                GetComponent<Text>().fontSize = hiFontSize;
                transform.localPosition = HindiPos;
            }
			else if(GetComponent<TextMesh>())
			{
                GetComponent<TextMesh>().fontSize = hiFontSize;
                gameObject.SetActive(false);
            }
			
		}
	}
}






