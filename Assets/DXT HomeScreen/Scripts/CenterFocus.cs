using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CenterFocus : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public float scaleFactor = 1.2f;
    public float normalScale = 1f;

    void Update()
    {
        float minDist = float.MaxValue;
        RectTransform centerItem = null;

        foreach (RectTransform item in content)
        {
            float dist = Mathf.Abs(item.position.x - scrollRect.viewport.position.x);
            if (dist < minDist)
            {
                minDist = dist;
                centerItem = item;
            }
        }

        foreach (RectTransform item in content)
        {
            if (item == centerItem)
                item.localScale = Vector3.Lerp(item.localScale, Vector3.one * scaleFactor, Time.deltaTime * 5f);
            else
                item.localScale = Vector3.Lerp(item.localScale, Vector3.one * normalScale, Time.deltaTime * 5f);
        }
    }
}
