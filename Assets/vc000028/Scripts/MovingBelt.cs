using UnityEngine;
using System.Collections;

public class MovingBelt : MonoBehaviour
{

	public float scrollSpeed = -0.5F;
	public static MovingBelt instance;
	public bool isLeftOffsetOn;
	public bool isRightOffsetOn;
	public Material mat;
	// Use this for initialization

	void Awake ()
	{
		instance = this;
	}

	void Start ()
	{
		isLeftOffsetOn = false;
		isRightOffsetOn = false;

	}

	// Update is called once per frame
	void Update ()
	{
//		if (isLeftOffsetOn) 
//		{
//			float offset = Time.time * scrollSpeed;
//			//this.gameObject.GetComponent<Renderer> ().material.mainTextureOffset = new Vector2 (0, offset);
//			this.gameObject.GetComponent<MeshRenderer> ().materials [3].SetTextureOffset ("_DetailAlbedoMap", new Vector2 (0f, offset));
//
//		}

		if (isRightOffsetOn) {
			float offset = Time.time * scrollSpeed;
//			this.gameObject.GetComponents<Renderer> ().Material [4].mainTextureOffset = new Vector2 (0, -offset);
//			this.gameObject.GetComponent<Renderer> ().materials [3].mainTextureOffset = new Vector2 (0, 5f);
			this.gameObject.GetComponent<MeshRenderer> ().materials [3].SetTextureOffset ("_DetailAlbedoMap", new Vector2 (0f, offset));
//			gameObject.GetComponents.mat.<Renderer>mainTextureOffset = new Vector2 (0, -offset);
		}
	}
}
