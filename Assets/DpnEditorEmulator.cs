// Update by Umety
// Preffered over DpnAuxiliaryMover.cs

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

/// Provides mouse-controlled head tracking emulation in the Unity editor.
public class DpnEditorEmulator : MonoBehaviour
{
#if UNITY_EDITOR
    private const string AXIS_MOUSE_X = "Mouse X";//"Mouse X";
    private const string AXIS_MOUSE_Y = "Mouse Y";//"Mouse Y";

    // Simulated neck model.  Vector from the neck pivot point to the point between the eyes.
    private static readonly Vector3 NECK_OFFSET = new Vector3(0, 0.075f, 0);

    // Use mouse to emulate head in the editor.
    // These variables must be static so that head pose is maintained between scene changes,
    // as it is on device.
    private float mouseX = 0;
    private float mouseY = 0;
    private float mouseZ = 0;

    public Vector3 HeadPosition { get; private set; }
    public Quaternion HeadRotation { get; private set; }


    public void Recenter()
    {
        mouseX = mouseZ = 0;  // Do not reset pitch, which is how it works on the phone.
        UpdateHeadPositionAndRotation();

        IEnumerator<Camera> validCameras = ValidCameras();
        while (validCameras.MoveNext())
        {
            Camera cam = validCameras.Current;
            cam.transform.localPosition = HeadPosition * cam.transform.lossyScale.y;
            cam.transform.localRotation = HeadRotation;
        }
    }

	private Vector2 lastAxis;
    public void UpdateEditorEmulation()
    {
		Vector2 axis = new Vector2(-(lastAxis.x - Input.mousePosition.x) * 0.1f, -(lastAxis.y - Input.mousePosition.y) * 0.1f);
		lastAxis = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        bool rolled = false;
        if (CanChangeYawPitch())
        {
//            mouseX += Input.GetAxis(AXIS_MOUSE_X) * 1;// 5;
//            if (mouseX <= -180)
//            {
//                mouseX += 360;
//            }
//            else if (mouseX > 180)
//            {
//                mouseX -= 360;
//            }
//            mouseY -= Input.GetAxis(AXIS_MOUSE_Y) * 1.2f;// 2.4f;
//            mouseY = Mathf.Clamp(mouseY, -85, 85);




			mouseX += axis.x * 1;// 5;
			            if (mouseX <= -180)
			            {
		                mouseX += 360;
			           }
			           else if (mouseX > 180)
		          {
			              mouseX -= 360;
		           }
			mouseY -= axis.y * 1.2f;// 2.4f;
			mouseY = Mathf.Clamp(mouseY, -85, 85);

        }
        else if (CanChangeRoll())
        {
            rolled = true;
            mouseZ += Input.GetAxis(AXIS_MOUSE_X) * 1;// 5;
            mouseZ = Mathf.Clamp(mouseZ, -85, 85);
        }

        if (!rolled)
        {
            // People don't usually leave their heads tilted to one side for long.
            mouseZ = Mathf.Lerp(mouseZ, 0, Time.deltaTime / (Time.deltaTime + 0.1f));
        }

        UpdateHeadPositionAndRotation();

        IEnumerator<Camera> validCameras = ValidCameras();
        while (validCameras.MoveNext())
        {
            Camera cam = validCameras.Current;
			Debug.Log(cam.name);
            cam.transform.localPosition = HeadPosition * cam.transform.lossyScale.y;
            cam.transform.localRotation = HeadRotation;
        }
    }

    void Update()
    {
        UpdateEditorEmulation();


    }


    private bool CanChangeYawPitch()
    {
        return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
    }

    private bool CanChangeRoll()
    {
        return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
    }

    private void UpdateHeadPositionAndRotation()
    {
        HeadRotation = Quaternion.Euler(mouseY, mouseX, mouseZ);
        HeadPosition = HeadRotation * NECK_OFFSET - NECK_OFFSET.y * Vector3.up;
    }

    private IEnumerator<Camera> ValidCameras()
    {
        for (int i = 0; i < Camera.allCameras.Length; i++)
        {
            Camera cam = Camera.allCameras[i];
            if (!cam.enabled || cam.stereoTargetEye == StereoTargetEyeMask.None)
            {
                continue;
            }

            yield return cam;
        }
    }
#endif
}
