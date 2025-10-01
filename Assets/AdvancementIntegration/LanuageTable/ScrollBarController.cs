using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollBarController : MonoBehaviour
{


    //public EventTrigger[] eventTriggerArr;

    //float prevScrollbarValue;
    //bool scrollAllowed_cd = false;
    //bool scrollAllowed_dd = false;
    //bool scrollAllowe_gv = false;
    //float speedFactor;
    //public string mode = "";
    //Vector2 previousPos;
    //float difference;

    //ScrollRect currentScrollRect;

    //void Awake()
    //{

    //    currentScrollRect = GetComponent<ScrollRect>();

    //    if (VrSelector.instance.IsCardboard)
    //        mode = "cardboard";
    //    else if (VrSelector.instance.IsDaydream)
    //        mode = "daydream";
    //    else if (VrSelector.instance.IsOculus)
    //        mode = "Oculus";
    //}


    //void OnEnable()
    //{
    //    //		if (mode == "daydream" || mode == "Oculus") {
    //    //			RefreshList1 ();
    //    //		}
    //    eventTriggerArr = currentScrollRect.GetComponentsInChildren<EventTrigger>();
    //    currentScrollRect.viewport.GetChild(0).localPosition = new Vector3(currentScrollRect.viewport.GetChild(0).localPosition.x, 0, currentScrollRect.viewport.GetChild(0).localPosition.z);
    //}

    //// DayDream Axis
    //public static float GetAxis(string direction)
    //{
    //    if (!GvrControllerInput.IsTouching)
    //        return 0;
    //    Vector2 input = (GvrController.TouchPos - new Vector2(0.5f, 0.5f)) * 2.0f;
    //    if (direction == "Horizontal")
    //    {
    //        return input.x;
    //    }
    //    else if (direction == "Vertical")
    //    {
    //        return -input.y;
    //    }
    //    else
    //    {
    //        return 0;
    //    }
    //}

    //void FixedUpdate()
    //{
    //    if (mode == "cardboard" && scrollAllowed_cd)
    //    {
    //        speedFactor = currentScrollRect.verticalScrollbar.size / 0.25f;
    //        currentScrollRect.verticalScrollbar.value += Mathf.Clamp(Input.GetAxis("Vertical") * Time.deltaTime * speedFactor, -1, 1);

    //    }
    //    else if (mode == "daydream" && scrollAllowed_dd)
    //    {

    //        speedFactor = currentScrollRect.verticalScrollbar.size / 0.25f;
    //        currentScrollRect.verticalScrollbar.value += Mathf.Clamp(GetAxis("Vertical") * Time.deltaTime * speedFactor, -1, 1);

    //        if (GvrControllerInput.ClickButtonDown)
    //        {

    //            if (eventTriggerArr != null)
    //            {
    //                for (int i = 0; i < eventTriggerArr.Length; i++)
    //                {
    //                    if (!eventTriggerArr[i].enabled)
    //                        eventTriggerArr[i].enabled = true;
    //                }
    //            }
    //            return;
    //        }

    //        if (GvrControllerInput.ClickButton)
    //        {
    //            //	print ("ClickButton");
    //            return;
    //        }

    //        if (GvrControllerInput.ClickButtonUp)
    //        {
    //            //	print ("ClickButtonUp");
    //            return;
    //        }


    //        if (GvrControllerInput.TouchUp)
    //        {

    //            if (eventTriggerArr != null)
    //            {
    //                for (int i = 0; i < eventTriggerArr.Length; i++)
    //                {
    //                    eventTriggerArr[i].enabled = true;
    //                }
    //            }

    //        }

    //        if (GvrControllerInput.IsTouching)
    //        {

    //            if (eventTriggerArr != null)
    //            {
    //                for (int i = 0; i < eventTriggerArr.Length; i++)
    //                {
    //                    eventTriggerArr[i].enabled = false;
    //                }
    //            }
    //        }


    //    }
    //    else if (mode == "Oculus" && scrollAllowe_gv)
    //    {
    //        speedFactor = currentScrollRect.verticalScrollbar.size / 0.25f;
    //        currentScrollRect.verticalScrollbar.value += Mathf.Clamp(OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y * Time.deltaTime * speedFactor, -1, 1);

    //        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
    //        {
    //            //	Debug.Log ("OVRInput.GetDown (OVRInput.Button.PrimaryTouchpad)");

    //            if (eventTriggerArr != null)
    //            {
    //                for (int i = 0; i < eventTriggerArr.Length; i++)
    //                {
    //                    if (!eventTriggerArr[i].enabled)
    //                        eventTriggerArr[i].enabled = true;
    //                }
    //            }
    //            return;
    //        }

    //        if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
    //        {
    //            //	Debug.Log ("OVRInput.Get (OVRInput.Button.PrimaryTouchpad)");
    //            return;
    //        }

    //        if (OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
    //        {
    //            //	Debug.Log ("OVRInput.GetUp (OVRInput.Button.PrimaryTouchpad)");
    //            return;
    //        }


    //        if (OVRInput.GetUp(OVRInput.Touch.PrimaryTouchpad))
    //        {
    //            //	Debug.Log ("OVRInput.GetUp (OVRInput.Touch.PrimaryTouchpad)");

    //            if (eventTriggerArr != null)
    //            {
    //                for (int i = 0; i < eventTriggerArr.Length; i++)
    //                {
    //                    eventTriggerArr[i].enabled = true;
    //                }
    //            }
    //            //				if (outlineBtnMgrs != null) {
    //            //					for (int i = 0; i < outlineBtnMgrs.Length; i++) {
    //            //						outlineBtnMgrs [i].OnPointerExit ();
    //            //					}
    //            //				}
    //            //
    //            //				if (btnMgrs != null) {
    //            //					for (int i = 0; i < btnMgrs.Length; i++) {
    //            //						btnMgrs [i].OnPointerExit ();
    //            //					}
    //            //				}
    //        }

    //        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad))
    //        {

    //            difference = Vector2.Distance(previousPos, OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad));

    //            if (difference > 0.2f)
    //            {

    //                //	Debug.Log ("OVRInput.Get (OVRInput.Touch.PrimaryTouchpad)");

    //                if (eventTriggerArr != null)
    //                {
    //                    for (int i = 0; i < eventTriggerArr.Length; i++)
    //                    {
    //                        eventTriggerArr[i].enabled = false;
    //                    }
    //                }
    //            }

    //            previousPos = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
    //        }
    //    }

    //}



    //public void OnPointerEnter()
    //{
    //    if (mode == "cardboard")
    //        scrollAllowed_cd = true;
    //    else if (mode == "daydream")
    //        scrollAllowed_dd = true;
    //    else if (mode == "Oculus")
    //        scrollAllowe_gv = true;

    //}

    //public void OnPointerExit()
    //{

    //    if (mode == "cardboard")
    //        scrollAllowed_cd = false;
    //    else if (mode == "daydream")
    //        scrollAllowed_dd = false;
    //    else if (mode == "Oculus")
    //        scrollAllowe_gv = false;
    //}
}

