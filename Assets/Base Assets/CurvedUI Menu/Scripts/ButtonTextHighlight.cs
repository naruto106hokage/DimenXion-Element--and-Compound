using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//using TMPro;
//using UnityEditor;

public class ButtonTextHighlight : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler, IPointerClickHandler
{
    void OnEnable()
    {
        for (int i = 0; i < GetComponentsInChildren<Text>().Length; i++)
        {
            GetComponentsInChildren<Text>()[i].color = new Color32 (255, 255, 255, 255);
        }
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        for (int i = 0; i < GetComponentsInChildren<Text>().Length; i++)
        {
            GetComponentsInChildren<Text>()[i].color =new Color32 (22, 143, 255, 255); //new Color32 (139, 178, 90, 255);
        }
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        for (int i = 0; i < GetComponentsInChildren<Text>().Length; i++)
        {
            GetComponentsInChildren<Text>()[i].color = new Color32 (255, 255, 255, 255);
        }
    }

    public void OnPointerClick (PointerEventData eventData)
    {
        for (int i = 0; i < GetComponentsInChildren<Text>().Length; i++)
        {
            GetComponentsInChildren<Text>()[i].color = new Color32 (255, 255, 255, 255);
        }
    }		
}
