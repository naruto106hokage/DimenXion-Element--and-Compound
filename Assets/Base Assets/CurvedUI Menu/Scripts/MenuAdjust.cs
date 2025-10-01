//-----------By Shantanu-------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MenuAdjust : MonoBehaviour
{
    [Range(0, 100)]
    public int spacing = 55;

    RectTransform[] objsRect;
    List<RectTransform> menuBtnsRect;
    Vector3[] pos;
    [HideInInspector]
    public int noOfLevels;

    public void Adjust()
    {
        objsRect = GetComponentsInChildren<RectTransform>();
        menuBtnsRect = new List<RectTransform>();

        for (int i = 0; i < objsRect.Length; i++)
        {
            if (objsRect[i].GetComponent<EventTrigger>() != null)
            {
                menuBtnsRect.Add(objsRect[i]);
                noOfLevels++;
            }
        }
        pos = new Vector3[noOfLevels];

        int startXMultiplier = -(noOfLevels - 1);

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = new Vector3(spacing * startXMultiplier, menuBtnsRect[0].localPosition.y, 0);
            startXMultiplier += 2;
        }

        for (int i = 0; i < noOfLevels; i++)
        {
            menuBtnsRect[i].localPosition = pos[i];
        }
    }
}
