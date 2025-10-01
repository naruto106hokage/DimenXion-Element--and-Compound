using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{

	public static GameManager instance;

	public GameObject button1;
	public GameObject button2;
	public GameObject button3;

	public Animation animFirstActivity;
	public Animation animSecondActivity;
	public Animation animSulphurIronMixure;
	public Animator animIronSulphide;
	public GameObject[] instructions;

	public GameObject selectBtn2;
	public GameObject selectBtn3;
	public GameObject moveTowardsNext;
	public GameObject moveTowardsNextFX;
	public GameObject restrictWall;

	public GameObject highlightBtn2;
	public GameObject beltHighlight;
	//public Transform hitBeakerPosition;
	public GameObject drag1Ques;
	public GameObject drag2Ques;
	public GameObject drag3Ques;
	public GameObject drag4Ques;
	public GameObject elementLEDText;
	public GameObject compoundLEDText, mixtureLEDText;

	EventTrigger _button1EventTrigger;
	EventTrigger _button2EventTrigger;
	EventTrigger _button3EventTrigger;
	public AnimationClip[] clips;
	public GameObject[] molecule;
	public GameObject ironSulfide;

	public Animation sulpherAnim;
	int yellowClickCounter;
	public GameObject sulphrFX, ironFX, fumeFx, burnerfx;
	public GameObject addedActivityInst1;
	// Use this for initialization
	void Start ()
	{
		yellowClickCounter = 0;
		instance = this;
		_button1EventTrigger = button1.GetComponent <EventTrigger> ();
		_button2EventTrigger = button2.GetComponent <EventTrigger> ();
		_button3EventTrigger = button3.GetComponent <EventTrigger> ();
	
		_button1EventTrigger.enabled = false;
		_button2EventTrigger.enabled = false;
		_button3EventTrigger.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void ClickOnButton1 ()
	{
		SoundManager.instance.PlayClickSound ();
		SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [0]);
		_button1EventTrigger.enabled = false;
		button1.transform.GetChild (1).gameObject.SetActive (false);
		animFirstActivity.Play ("Mixing1"); 
		animSulphurIronMixure.Play ();
		Invoke ("AppearInstruction2", animFirstActivity.clip.length); 

	}

	public void ClickOnButton2 ()
	{
		SoundManager.instance.PlayClickSound ();
		_button2EventTrigger.enabled = false;
		SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [2]);

		button2.transform.GetChild (1).gameObject.SetActive (false);
		animIronSulphide.enabled = true;
		fumeFx.SetActive (true);
		LanguageHandler.instance.PlayVoiceOver ("L1_Btn_Instruction3");
		Invoke ("AppearInstruction3", 5f); 

	}

	public void ClickOnButton3 ()
	{

		_button3EventTrigger.enabled = false;
		button3.transform.GetChild (1).gameObject.SetActive (false);
		selectBtn3.SetActive (false); 
		animFirstActivity.Play ("Collection");
		changecolor.instance.play ();
		animFirstActivity ["Collection"].speed = .4f;
		Invoke ("MoveTowardsNext", 5f); 
		Invoke ("Delay", 3f); 

	}

	void Delay ()
	{
		ironSulfide.SetActive (false);
	}

	void MoveTowardsNext ()
	{
		addedActivityInst1.SetActive (true);
//		moveTowardsNext.SetActive (true); 
//		restrictWall.SetActive (false);
//		moveTowardsNextFX.SetActive (true); 
	}

	public void OnClickAddedInst1 ()
	{
		SoundManager.instance.PlayClickSound ();
		addedActivityInst1.SetActive (false);
		moveTowardsNext.SetActive (true); 
		restrictWall.SetActive (false);
		moveTowardsNextFX.SetActive (true); 
	}

	void AppearInstruction3 ()
	{
		burnerfx.SetActive (false);
		instructions [1].SetActive (true); 
		MovementController.instance.enabled = false;

	}

	public void AppearInstruction2 ()
	{

		instructions [0].SetActive (true);
	}

	public void ClickOnInstruction2 ()
	{


		_button3EventTrigger.enabled = true;
	}


	public void ClickOnYelloBtn ()
	{
		SoundManager.instance.PlayClickSound ();
		button3.GetComponent<BoxCollider> ().enabled = false;
		yellowClickCounter++;
		if (yellowClickCounter == 1) {
			SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [1]);

			StartCoroutine (AfterClickOnInstruction2 ()); 

		} else if (yellowClickCounter == 2) {
			SfxPlayVoice.spv.PlayAudioClip (SfxPlayVoice.spv.audioclip [3]);

			ClickOnButton3 ();
		}
	
	}

	public void ClickOnInstruction3 ()
	{
		button3.GetComponent<BoxCollider> ().enabled = true;

		selectBtn3.SetActive (true); 
		_button3EventTrigger.enabled = true;
	}

	IEnumerator AfterClickOnInstruction2 ()
	{

		animFirstActivity.Play ("Reaction"); 
		animFirstActivity ["Reaction"].speed = .4f;

		yield return new WaitForSeconds (5f);

		selectBtn2.SetActive (true); 
		highlightBtn2.SetActive (true); 
		_button2EventTrigger.enabled = true;
	}

	public IEnumerator Drag1 ()
	{
		
		drag1Ques.SetActive (false); 
		beltHighlight.SetActive (false);
		//yield return new WaitForSeconds (.4f);

		animSecondActivity.clip = clips [0];
		animSecondActivity.Play ();

		yield return new WaitForSeconds (5f);
		MovingBelt.instance.isRightOffsetOn = false;
		instructions [3].SetActive (true); 
		elementLEDText.SetActive (true); 
		float len = clips [0].length - 5f;
		yield return new WaitForSeconds (len);

		molecule [0].GetComponent<RotateMolecule> ().enabled = true;
	}

	public IEnumerator Drag2 ()
	{
		beltHighlight.SetActive (false);

		drag2Ques.SetActive (false); 
		//yield return new WaitForSeconds (2f);
		animSecondActivity.clip = clips [1];
		animSecondActivity.Play ();

		yield return new WaitForSeconds (5f);
		MovingBelt.instance.isRightOffsetOn = false;
		instructions [4].SetActive (true); 

		elementLEDText.SetActive (true); 
		float len = clips [0].length - 5f;
		yield return new WaitForSeconds (len);
	
		molecule [1].GetComponent<RotateMolecule> ().enabled = true;
	}

	public IEnumerator Drag3 ()
	{
		beltHighlight.SetActive (false);

		drag4Ques.SetActive (false); 
		//yield return new WaitForSeconds (2f);
		animSecondActivity.clip = clips [3];
		animSecondActivity.Play ();

		yield return new WaitForSeconds (5f);
		MovingBelt.instance.isRightOffsetOn = false;
		instructions [6].SetActive (true); 

		mixtureLEDText.SetActive (true); 
		float len = clips [0].length - 5f;
		yield return new WaitForSeconds (len);

		//molecule [2].GetComponent<RotateMolecule> ().enabled = true;

	}

	public IEnumerator Drag4 ()
	{
		beltHighlight.SetActive (false);

		drag3Ques.SetActive (false); 
		//yield return new WaitForSeconds (1.5f);
		animSecondActivity.clip = clips [2];
		animSecondActivity.Play ();
	
		yield return new WaitForSeconds (5f);
		MovingBelt.instance.isRightOffsetOn = false;
		instructions [5].SetActive (true); 

		compoundLEDText.SetActive (true); 
		float len = clips [0].length - 5f;
		yield return new WaitForSeconds (len);
		molecule [2].GetComponent<RotateMolecule> ().enabled = true;

	}
}
