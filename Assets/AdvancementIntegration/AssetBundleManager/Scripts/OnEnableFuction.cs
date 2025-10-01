using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnEnableFuction : MonoBehaviour {
	
	void OnEnable()
	{
		
		MyUiCltr.instance.Reset ();
	}
}
