/************************************************************************************

Copyright   :   Copyright 2017 DeePoon LLC. All Rights reserved.

DPVR Developer Website: http://developer.dpvr.cn

************************************************************************************/

using UnityEngine;

namespace dpn
{
/// This script provides an interface for pointer based input used with
/// the DpnPointerInputModule script.
///
/// It provides methods called on pointer interaction with in-game objects and UI,
/// trigger events, and 'BaseInputModule' class state changes.
///
/// To have the methods called, an instance of this (implemented) class must be
/// registered with the **DpnPointerManager** script on 'OnEnable' by calling
/// DpnPointerManager.OnPointerCreated.
/// A registered instance should also unregister itself at 'OnDisable' calls
/// by setting the **DpnPointerManager.Pointer** static property
/// to null.
///
/// This class is expected to be inherited by pointers doing 1 of 2 things:
/// 1. Responding to movement of the users head (Cardboard gaze-based-pointer).
/// 2. Responding to the movement of the daydream controller (Daydream 3D pointer).
	public interface IDpnPointer
	{
		/// This is called when the 'BaseInputModule' system should be enabled.
		void OnInputModuleEnabled();

		/// This is called when the 'BaseInputModule' system should be disabled.
		void OnInputModuleDisabled();

		/// Called when the pointer is facing a valid GameObject. This can be a 3D
		/// or UI element.
		///
		/// The targetObject is the object the user is pointing at.
		/// The intersectionPosition is where the ray intersected with the targetObject.
		/// The intersectionRay is the ray that was cast to determine the intersection.
		void OnPointerEnter(GameObject targetObject, Vector3 intersectionPosition,
		   Ray intersectionRay, bool isInteractive);

		/// Called every frame the user is still pointing at a valid GameObject. This
		/// can be a 3D or UI element.
		///
		/// The targetObject is the object the user is pointing at.
		/// The intersectionPosition is where the ray intersected with the targetObject.
		/// The intersectionRay is the ray that was cast to determine the intersection.
		void OnPointerHover(GameObject targetObject, Vector3 intersectionPosition,
			Ray intersectionRay, bool isInteractive);

		/// Called when the pointer no longer faces an object previously
		/// intersected with a ray projected from the camera.
		/// This is also called just before **OnInputModuleDisabled** and may have have any of
		/// the values set as **null**.
		void OnPointerExit(GameObject targetObject);

		/// Called when a click is initiated.
		void OnPointerClickDown();

		/// Called when click is finished.
		void OnPointerClickUp();

		/// Returns the max distance this pointer will be rendered at from the camera.
		/// This is used by DpnBasePointerRaycaster to calculate the ray when using
		/// the default "Camera" RaycastMode. See DpnBasePointerRaycaster.cs for details.
		float GetMaxPointerDistance();

		/// Returns the transform that represents this pointer.
		/// It is used by DpnBasePointerRaycaster as the origin of the ray.
		Transform GetPointerTransform();

		/// Return the radius of the pointer. This is currently
		/// only used by DpnGaze. It is used when searching for
		/// valid gaze targets. If a radius is 0, the DpnGaze will use a ray
		/// to find a valid gaze target. Otherwise it will use a SphereCast.
		/// The *innerRadius* is used for finding new targets while the *outerRadius*
		/// is used to see if you are still nearby the object currently looked at
		/// to avoid a flickering effect when just at the border of the intersection.
		void GetPointerRadius(out float innerRadius, out float outerRadius);

		///  get the position on the screen
        Vector2 GetScreenPosition();
	}
}

