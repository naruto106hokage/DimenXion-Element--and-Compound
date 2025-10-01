using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changecolor : MonoBehaviour
{
	public static changecolor instance;
	// Use this for initialization
	void Start ()
	{
		instance = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void Stop ()
	{
		GameManager.instance.sulpherAnim ["ShaderChange"].speed = 0;
	}

	public void play ()
	{
		GameManager.instance.sulpherAnim ["ShaderChange"].speed = 1;
	}
}
