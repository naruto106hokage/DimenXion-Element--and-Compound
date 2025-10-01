using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ToScaleRaysandRotate : MonoBehaviour
{
	public static ToScaleRaysandRotate instance;

	public GameObject raysScalenRotate;

	// Use this for initialization
	void Start ()
	{
		instance = this;
	}


	/// <summary>
	/// to scale and rotate rays  continuesly
	/// </summary>
	void Update ()
	{
		raysScalenRotate.transform.DOScale (Vector3.one, 1f);
		raysScalenRotate.transform.Rotate (new Vector3 (0f, 360f, 0f) * (Time.deltaTime / 20));

	}


}
