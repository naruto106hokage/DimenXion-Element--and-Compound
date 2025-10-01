using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ButtonImageHighlight : MonoBehaviour
{

    public Image Container;
    public Sprite NextImage;
    public Sprite NextHoverImage;
    

    public void OnHoverEnter()
    {

        Container.sprite = NextHoverImage;

    }

    public void OnHoverExit ()
    {
        
       Container.sprite = NextImage;
       
    }

	public void OnClick ()
    {
        
       Container.sprite = NextImage;
       
    }

}

