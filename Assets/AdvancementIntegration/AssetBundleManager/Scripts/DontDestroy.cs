using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour {
    public static DontDestroy instance;
	// Use this for initialization
	void Start () 
    {
        DontDestroyOnLoad(this.gameObject);
        if(instance != null && instance != this)
        {

            Destroy(instance.gameObject);
            instance = null;
        }


        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
		
	}
}
