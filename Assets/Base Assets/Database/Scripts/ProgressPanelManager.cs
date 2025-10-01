using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPanelManager : MonoBehaviour {


    public static ProgressPanelManager ppm;
    private Image fillImage;

    void Awake ( )
    {

        ppm = this;

    }

    public void ShowProgressPanel ( )
    {

        transform.GetChild ( 0 ).gameObject.SetActive ( true );

    }

    public void HideProgressPanel ( )
    {

        transform.GetChild(0).gameObject.SetActive ( false );

    }

    public void ShowProgressPanelForSometime ( )
    {

        ShowProgressPanel ( );
        Invoke ( "HideProgressPanel" , 3f );

    }

}
