using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dpn;

public class Dpn3DofArmModel : MonoBehaviour {

    // Use this for initialization
	public static Dpn3DofArmModel instance;

	private Transform flipController;
    
    [SerializeField]
    private Transform rightElbow;

    [SerializeField]
    private Transform rightWrist;

    [SerializeField]
    private Transform leftElbow;

    [SerializeField]
    private Transform leftWrist;

    [SerializeField]
    private Transform controller;

	[SerializeField]
	private Transform VRPlayer;

    public int interactiveHand = 0;

	[SerializeField]
	private Transform parent;

	Vector3 offSet,wristPos;
    void Start () {
		instance = this;

		flipController = dpn.DpnCameraRig._instance.transform.Find ("FlipController(Clone)");
    }

	// Update is called once per frame
	void Update () {

    }

    void LateUpdate()
    {
        Quaternion cameraRigRot = dpn.DpnCameraRig._instance.transform.rotation;
		float y = dpn.DpnCameraRig._instance.GetPose().eulerAngles.y; // >>>Original

		flipController.eulerAngles = Vector3.zero;

		transform.localRotation =cameraRigRot *  Quaternion.Euler(0, y, 0);

        if (interactiveHand == 0)
        {
			
            rightElbow.transform.rotation = cameraRigRot * dpn.DpnDaydreamController.Orientation; // >>>Original
            controller.rotation = rightWrist.transform.rotation;
			controller.position = rightWrist.transform.position;

        }
        else
        {
            leftElbow.transform.rotation = cameraRigRot * dpn.DpnDaydreamController.Orientation;

            controller.rotation = leftWrist.transform.rotation;
            controller.position = leftWrist.transform.position;
        }
    }
}
