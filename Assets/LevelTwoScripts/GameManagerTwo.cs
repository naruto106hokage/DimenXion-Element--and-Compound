using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManagerTwo : MonoBehaviour
{

	public static GameManagerTwo instance;
	public GameObject gvrRecticle;
	public bool check = false;
	public bool checkOne = false;
	public GameObject tableScreen;
	public bool nextActivity = false;
	[Header ("Instructions And Labels")]
	public GameObject[] cameraInInstructions;
	public GameObject[] instructions;
	public GameObject[] labels;
	[Header ("HighlightAreas")]
	public GameObject HighlightedAreaOne;
	public GameObject HighlightedAreaTwo;
	[Header ("ActivityAreaCollider")]
	public GameObject ActivityColliderOne;
	public GameObject ActivityColliderTwo;

	[Header ("TableOneObjects And Associated Collider")]
	public GameObject magnet;
	public GameObject ironSulphide;
	public GameObject ironMixture;
	public GameObject ironMixture_cntrl_S_grp;
	public GameObject ironMixture_cntrl_N_grp;
	public GameObject ironSulphidePos;
	public GameObject ironMixturePos;
	public GameObject WatchGlassIronSulphide;
	public GameObject WatchGlassMixture;
	[Header ("TableTwoObjects And Associated Collider")]
	public GameObject bottleCap;
	public GameObject solutionBottle;
	public GameObject solutionBottleCollider;
	public GameObject cylinderCollider;
	public GameObject graduateCylinder;
	public GameObject graduateCylinderColllider;
	public GameObject graduateCylinderHalfFillColllider;
	public GameObject beakerACollider;
	public GameObject beakerBCollider;
	public GameObject graduateCylinderBlend;
	[Header ("Animation And Clips")]

	public Animation anim;
	public Animation anim1;
	public AnimationClip[] tableOneClips;

	[Header ("DetailTableScreen")]
	public GameObject[] details;

	[Header ("QuestionPanel")]
	public GameObject q1;
	public GameObject q2;
	public GameObject q3;
	public GameObject[] questionsOk;

	public Transform d;

	// Use this for initialization
	void Start ()
	{
		instance = this;

	}
	

}
