using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppVersionDisplay : MonoBehaviour {

	public Text version;
	bool istrue;

	void Start () 
	{
		version.text = "V "+Application.version; 
		istrue = false;
	}

	void Update()
	{
		if (!istrue) {
			transform.GetChild (0).transform.GetComponent<Text> ().horizontalOverflow = HorizontalWrapMode.Overflow;
			transform.GetChild (0).transform.GetComponent<Text> ().verticalOverflow = VerticalWrapMode.Overflow;
			transform.GetChild (0).transform.GetComponent<Text> ().resizeTextForBestFit = false;
			istrue = true;
		}
	}

}
