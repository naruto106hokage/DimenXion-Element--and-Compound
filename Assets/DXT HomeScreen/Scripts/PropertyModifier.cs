using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyModifier : MonoBehaviour {
    public GameObject parent;

	// Use this for initialization
	void Start () {
        changeColor();
        changeImage();
    }

    public void changeColor()
    {
        //Text[] texts = FindObjectsOfType<Text>();
        Text[] texts = Resources.FindObjectsOfTypeAll<Text>();
        foreach (Text text in texts)
        {
            if(IsInScene(text.gameObject))
            text.color = Color.black;
        }
    }

    private bool IsInScene(GameObject go)
    {
        return go.hideFlags == HideFlags.None && go.scene.name!= null && go.scene.name != "";
    }
    public void changeImage()
    {
        Image[] images = Resources.FindObjectsOfTypeAll<Image>();
        foreach (Image image in images)
        {
            if (IsInScene(image.gameObject))
                image.type = Image.Type.Simple;
        }
    }
}
