using System.Collections.Generic;
using UnityEngine;

public class ReverseLevelContainerItems : MonoBehaviour
{

    [SerializeField]private List<RectTransform> childCanvasMenus;
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            childCanvasMenus.Add(transform.GetChild(i).gameObject.GetComponent<RectTransform>());
        }
    }
    
    private void Start()
    {
        if (LanguageHandler.instance.IsRightToLeft)
        {
            for (int i = 0; i < childCanvasMenus.Count; i++)
            {
                childCanvasMenus[i].transform.SetParent(null);
            }
            for (int i = childCanvasMenus.Count - 1; i >= 0; i--)
            {
                childCanvasMenus[i].transform.SetParent(this.gameObject.transform);
            }
        }

    }

    public void Reverse()
    {
        if (LanguageHandler.instance.IsLeftToRight)
        {
            
        }
        else
        {
           
        }
    }
}
