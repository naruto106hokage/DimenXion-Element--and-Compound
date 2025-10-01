using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryTrigger : MonoBehaviour
{

	public GameObject restrict1;
	public GameObject highlight1;
	public GameObject highlight2;
	public GameObject instruction1;
	public GameObject instruction4;
	public GameObject moveTowards;
	public GameObject moveTowardsNext;
	public GameObject labelTextActivity2;
	public GameObject labelText2Activity2;
	public GameObject sulfurTxt, ironTxt;
	public GameObject levl1, levl2;
	// Use this for initialization
	void Start ()
	{

		print ("hiii..."); 
	}

	public void OnTriggerEnter (Collider col)
	{

		if (col.CompareTag ("col1")) {

			moveTowards.SetActive (false); 
			instruction1.SetActive (true); 
			sulfurTxt.SetActive (true);
			ironTxt.SetActive (true);
			levl1.SetActive (true);
			levl2.SetActive (true);

			highlight1.SetActive (false);
			restrict1.SetActive (true);
			MovementController.instance.enabled = false;
		} else if (col.CompareTag ("col2")) {

			moveTowardsNext.SetActive (false); 

			highlight2.SetActive (false);
			restrict1.SetActive (true);
			labelTextActivity2.SetActive (true); 

			StartCoroutine (AppearNextLebelText ()); 


		}

	}

	IEnumerator AppearNextLebelText ()
	{

		yield return new WaitForSeconds (2f);

		//GameManager.instance.animSecondActivity.Play ("sampleDish");
		GameManager.instance.animSecondActivity.clip = GameManager.instance.clips [4];
		GameManager.instance.animSecondActivity.Play ();
		SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [4]);

		Invoke ("Vfx", .8f);
		yield return new WaitForSeconds (2.5f);
		labelText2Activity2.SetActive (true); 
		yield return new WaitForSeconds (1f);
		instruction4.SetActive (true); 
	}

	void Vfx ()
	{
		GameManager.instance.ironFX.SetActive (true);
		GameManager.instance.sulphrFX.SetActive (true);
	}
}
