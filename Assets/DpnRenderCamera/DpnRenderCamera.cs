using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dpn;

[RequireComponent(typeof(Camera))]
public class DpnRenderCamera : MonoBehaviour
{
    internal Camera _left_eye;
    internal Camera _right_eye;
    internal Camera _center_eye;

    internal Camera _Render_left_eye;
    internal Camera _Render_right_eye;
    internal Camera _Render_center_eye;
    // Use this for initialization
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        _left_eye = DpnCameraRig._instance._left_eye;
        _right_eye = DpnCameraRig._instance._right_eye;
        _center_eye = DpnCameraRig._instance._center_eye;

        _Render_center_eye = GetComponent<Camera>();

        GameObject tmp = new GameObject("Render_right_eye", typeof(Camera));
        tmp.transform.parent = _right_eye.transform;
        tmp.transform.localPosition = Vector3.zero;
        tmp.transform.localEulerAngles = Vector3.zero;
        _Render_right_eye = tmp.GetComponent<Camera>();
        _Render_right_eye.cullingMask = _Render_center_eye.cullingMask;
        _Render_right_eye.depth = _Render_center_eye.depth;

        tmp = new GameObject("Render_left_eye", typeof(Camera));
        tmp.transform.parent = _left_eye.transform;
        tmp.transform.localPosition = Vector3.zero;
        tmp.transform.localEulerAngles = Vector3.zero;
        _Render_left_eye = tmp.GetComponent<Camera>();
        _Render_left_eye.cullingMask = _Render_center_eye.cullingMask;
        _Render_left_eye.depth = _Render_center_eye.depth;

        CopyCameraProps(_Render_center_eye, _center_eye);
        CopyCameraProps(_Render_left_eye, _left_eye);
        CopyCameraProps(_Render_right_eye, _right_eye);

#if UNITY_EDITOR
        _left_eye.enabled = false;
        _right_eye.enabled = false;
        _Render_right_eye.enabled = false;
        _Render_left_eye.enabled = false;
#else
        _Render_center_eye.enabled = false;
#endif
    }

    void OnEnable()
    {
        if (_Render_left_eye)
            _Render_left_eye.gameObject.SetActive(true);
        if (_Render_right_eye)
            _Render_right_eye.gameObject.SetActive(true);

        Camera.onPreRender += UpdateCameraTexture;
    }

    void OnDisable()
    {
        if (_Render_left_eye)
            _Render_left_eye.gameObject.SetActive(false);
        if (_Render_right_eye)
            _Render_right_eye.gameObject.SetActive(false);

        Camera.onPreRender -= UpdateCameraTexture;
    }

    private void UpdateCameraTexture(Camera cam)
    {
        if (cam.name == "Render_left_eye")
            _Render_left_eye.targetTexture = _left_eye.targetTexture;
        if (cam.name == "Render_right_eye")
            _Render_right_eye.targetTexture = _right_eye.targetTexture;
    }

    void CopyCameraProps(Camera To, Camera From)
    {
        To.clearFlags = CameraClearFlags.Depth;
        To.fieldOfView = From.fieldOfView;
        To.aspect = From.aspect;
        To.nearClipPlane = From.nearClipPlane;
        To.farClipPlane = From.farClipPlane;
        To.rect = From.rect;
        To.targetTexture = From.targetTexture;
        To.useOcclusionCulling = From.useOcclusionCulling;
        To.allowHDR = From.allowHDR;
        To.allowMSAA = From.allowMSAA;
        To.allowDynamicResolution = From.allowDynamicResolution;
    }
}
