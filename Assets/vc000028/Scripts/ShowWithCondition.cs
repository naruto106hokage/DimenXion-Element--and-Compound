using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWithCondition : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.SetActive(PlayerPrefs.GetString("currentLanguage") == "mr-IN");
	}
	
}
