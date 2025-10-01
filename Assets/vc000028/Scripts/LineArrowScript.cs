using UnityEngine;
using System.Collections;

public class LineArrowScript : MonoBehaviour {

	// Use this for initialization

    internal LineRenderer lr;
    public Transform target;

    public  Material mat;

	void Update () {


        if (!gameObject.GetComponent<LineRenderer> ()) {


            lr = gameObject.AddComponent<LineRenderer> () as LineRenderer;



        } 


        else {
            lr = gameObject.GetComponent<LineRenderer> ();

        }



        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lr.receiveShadows = false;
        lr.material = mat;
        lr.SetWidth (0.05f, 0.02f);
        lr.SetVertexCount (2);
        lr.SetPosition (0, transform.position);
        lr.SetPosition (1, target.position);
        //lr.SetColors(Color.white, Color.white);

	
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
}
