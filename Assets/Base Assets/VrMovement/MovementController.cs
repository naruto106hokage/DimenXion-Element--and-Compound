using UnityEngine;
using dpn;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    public static MovementController instance;
    public float speed = 3.0F;
    private CharacterController controllor;
    public static bool IsMoving = false;
    public static float curSpeed;
    private Transform vrHead;

    Vector3 forward;
    Vector2 tp;

    void Start()
    {
        instance = this;
        controllor = GetComponent<CharacterController>();
        vrHead = DpnCameraRig._instance._center_eye.transform;
    }

    void Update()
    {
#if UNITY_EDITOR
        tp = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        forward = vrHead.TransformDirection(Vector3.forward);
        curSpeed = speed * tp.y;
        controllor.SimpleMove(forward * curSpeed);

#else
        if (!DpnDaydreamController.IsTouching)
        {
            tp = Vector3.zero;
            return;
        }
        tp = DpnDaydreamController.TouchPos;


        if (Mathf.Abs(tp.x) <= Mathf.Abs(tp.y))
        {
            if (Mathf.Abs(tp.y) > 0.5f)
            {
                forward = vrHead.TransformDirection(Vector3.forward);
                curSpeed = speed * -tp.y;
                controllor.SimpleMove(forward * curSpeed);
            }
        }
#endif
    }
}