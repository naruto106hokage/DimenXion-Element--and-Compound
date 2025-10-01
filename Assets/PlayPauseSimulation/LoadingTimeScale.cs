using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingTimeScale : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void OnDisable()
    {

        Time.timeScale = 1;
        Debug.Log("Done Done Done");
    }


}
