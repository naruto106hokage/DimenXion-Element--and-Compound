
using System;
using UnityEngine;

namespace dpn
{
    public class DpnFinalRenderObject : MonoBehaviour
    {
        Renderer[] _renderers = null;
        DpnCamera3 _camera3 = null;

        int LAYER = 31;

        public bool applyToChildren = true;

        void Awake()
        {
            _camera3 = gameObject.AddComponent<DpnCamera3>();
            _camera3.leftTransform = DpnCameraRig._instance._left_transform;
            _camera3.rightTransform = DpnCameraRig._instance._right_transform;
            _camera3.centerTransform = DpnCameraRig._instance._center_transform;

            _camera3.clearFlags = CameraClearFlags.Nothing;
            _camera3.depth = 100.0f;
            _camera3.cullingMask = 1 << LAYER;

            gameObject.layer = LAYER;

            if (applyToChildren)
            {
                _renderers = GetComponentsInChildren<Renderer>();
                for (int i = 0; i < gameObject.transform.childCount; ++i)
                {
                    gameObject.transform.GetChild(i).gameObject.layer = LAYER;
                }
            }
            else
            {
                _renderers = new Renderer[1];
                _renderers[0] = GetComponent<Renderer>();
            }

            Camera.onPreCull += OnCameraPreCull;
        }

        void Start()
        {
           
        }

        bool _isVisable = true;

        void OnCameraPreCull(Camera camera)
        {
            if (_renderers == null)
                return;

            if (_camera3.ContainsCamera(camera))
            {
                if (!_isVisable)
                {
                    EnableRenderers(true);
                    _isVisable = true;
                }
            }
            else
            {
                if (_isVisable)
                {
                    EnableRenderers(false);
                     _isVisable = false;
                }
            }
        }

        void EnableRenderers(bool enabled)
        {
            foreach (Renderer renderer in _renderers)
            {
                renderer.enabled = enabled;
            }
        }

        void OnDestroy()
        {
            if (_renderers != null)
            {
                Array.Clear(_renderers, 0, _renderers.Length);
                _renderers = null;
            }
        }
    }
}