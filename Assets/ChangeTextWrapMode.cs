using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextWrapMode : MonoBehaviour {


	void Start ()
	{
		Text[] t=(Text[]) GameObject.FindObjectsOfType (typeof(Text));
		Text[] tNew = Resources.FindObjectsOfTypeAll<Text>();
		foreach(Text _t in tNew)
		{
            _t.verticalOverflow = VerticalWrapMode.Truncate;
            _t.horizontalOverflow = HorizontalWrapMode.Wrap;

            _t.resizeTextForBestFit = true;
			_t.resizeTextMaxSize = _t.fontSize;
			_t.resizeTextMinSize = 5;
           // Debug.Log("font size "+_t.fontSize+ " MaxFont size "+_t.resizeTextMaxSize);
            

		}
	}


}
