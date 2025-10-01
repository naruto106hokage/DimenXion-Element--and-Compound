using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToRotateBase : MonoBehaviour
{
	public static ToRotateBase instance;
	// Use this for initialization
	void Start ()
	{
		instance = this;
	}

	/// <summary>
	/// to   continues rotation base.
	/// </summary>
	void Update ()
	{
		transform.Rotate (new Vector3 (0, 360, 0) * Time.deltaTime / 20f);
	}
		

}
