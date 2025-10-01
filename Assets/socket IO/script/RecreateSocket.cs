using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using socket.io;

public class RecreateSocket : MonoBehaviour
{
	public static RecreateSocket INSTANCE;
	public int RecreateCount = 0;
	public GameObject socketPrefab;
	public RenderTexture screenCaptureTex;

	void Awake ()
	{
		INSTANCE = this;
		GameObject.DontDestroyOnLoad (gameObject);
	}


	// Use this for initialization
	void Start ()
	{
		//InvokeRepeating ("DistroyAndCreateSocket", 10, 10);
		//Invoke ("DistroyAndCreateSocket", 20);
	}
	
	// Update is called once per frame
	void Update ()
	{
	}


	void DistroySocket ()
	{
		GameObject.DestroyImmediate (socketPrefab);
		GameObject.DestroyImmediate (SocketManager.Instance.gameObject);
	}

	void CreateSocket ()
	{		
		socketPrefab = new GameObject ();
		socketPrefab.name = "SocketPrefab";
		socketPrefab.AddComponent <SocketIOScript> ().screenCaptureTex = screenCaptureTex;
	}


	public void DistroyAndCreateSocket ()
	{
		Debug.Log ("DC100350 Recreating Socket: " + (++RecreateCount));

		if (RecreateCount < 10) {
			DistroySocket ();
			CreateSocket ();
		}
	}

}
