/************************************************************************************

Copyright   :   Copyright 2015-2017 DeePoon LLC. All Rights reserved.

DPVR Developer Website: http://developer.dpvr.cn

************************************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace dpn
{
/// Base implementation of IDpnPointer
///
/// Automatically registers pointer with DpnPointerManager.
/// Uses transform that this script is attached to as the pointer transform.
///
	public abstract class DpnBasePointer : MonoBehaviour, IDpnPointer
	{
        // Update by Umety
        public static DpnBasePointer _instance;
        public bool LockDistance = false;
        //=============================

        protected virtual void OnEnable()
		{
			DpnPointerManager.OnPointerCreated(this);
            // Update by Umety
            _instance = this;
            //=============
        }

        protected virtual void OnDisable()
        {
            DpnPointerManager.OnPointerDestroy(this);
        }

        /// Declare methods from IDpnPointer
        public abstract void OnInputModuleEnabled();

		public abstract void OnInputModuleDisabled();

		public abstract void OnPointerEnter(GameObject targetObject, Vector3 intersectionPosition,
			Ray intersectionRay, bool isInteractive);

		public abstract void OnPointerHover(GameObject targetObject, Vector3 intersectionPosition,
			Ray intersectionRay, bool isInteractive);

		public abstract void OnPointerExit(GameObject targetObject);

		public abstract void OnPointerClickDown();

		public abstract void OnPointerClickUp();

		public abstract float GetMaxPointerDistance();

		public abstract void GetPointerRadius(out float innerRadius, out float outerRadius);

		public virtual Transform GetPointerTransform()
		{
			return transform;
		}

		public Vector2 GetScreenPosition()
		{
			return DpnCameraRig.WorldToScreenPoint(transform.position);
		}

		protected float tiltedAngle = 0.0f;

		virtual public void SetTitledAngle(float degree)
		{
			tiltedAngle = degree;
		}

		public Ray GetRay()
		{
			Transform pointerTransform = DpnPointerManager.Pointer.GetPointerTransform();

			Camera centerCamera = DpnCameraRig._instance._center_eye;
			Matrix4x4 matrixController =  pointerTransform.localToWorldMatrix;

			Ray castRay;
			Matrix4x4 matrixRayEnding = new Matrix4x4();
			matrixRayEnding.SetTRS(Vector3.zero, Quaternion.Euler(-tiltedAngle, 0,0), Vector3.one);
			matrixRayEnding = matrixController * matrixRayEnding;

			Vector3 rayPointerStart = matrixRayEnding.GetColumn(3);
			Vector3 rayPointerEnd = rayPointerStart + ((Vector3)matrixRayEnding.GetColumn(2) * DpnPointerManager.Pointer.GetMaxPointerDistance());

			Vector3 cameraLocation = centerCamera.transform.position;
			Vector3 finalRayDirection = rayPointerEnd - cameraLocation;
			finalRayDirection.Normalize();

			Vector3 finalRayStart = cameraLocation + (finalRayDirection * centerCamera.nearClipPlane);

			castRay = new Ray(finalRayStart, finalRayDirection);
			return castRay;
		}
	}
}
