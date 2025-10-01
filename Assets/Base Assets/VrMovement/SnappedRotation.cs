using UnityEngine;
using dpn;

public class SnappedRotation : MonoBehaviour
{
    public float rotateAngle = 20;

    Vector2 tp;

    void Update()
    {
		
#if UNITY_EDITOR
        tp = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetButtonUp("Horizontal"))
        {
            if (tp.x > 0)
                transform.Rotate(0, rotateAngle, 0);
            else if (tp.x < 0)
                transform.Rotate(0, -rotateAngle, 0);
        }

#else   
		if ( DpnDaydreamController.ClickButtonDown)
        {
            if (Mathf.Abs(tp.x) >= Mathf.Abs(tp.y))
            {
                if (tp.x > 0.5f)
                    transform.Rotate(0, rotateAngle, 0);
                else if (tp.x < -0.5f)
                    transform.Rotate(0, -rotateAngle, 0);
            }
        }
        tp = DpnDaydreamController.TouchPos;
#endif
    }
}