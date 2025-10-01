/************************************************************************************

Copyright   :   Copyright 2015-2017 DeePoon LLC. All Rights reserved.

DPVR Developer Website: http://developer.dpvr.cn

************************************************************************************/

using UnityEngine;
using System.Collections;

namespace dpn
{
/// DpnPointerManager is a standard interface for
/// controlling which IDpnPointer is being used
/// for user input affordance.
///
	public class DpnPointerManager : MonoBehaviour
	{
		private static DpnPointerManager instance;

		/// Change the IDpnPointer that is currently being used.
		public static IDpnPointer Pointer
		{
			get
			{
                return instance != null ? instance.pointer : null;
			}
			set
			{
				if (instance == null || instance.pointer == value)
				{
					return;
				}

				instance.pointer = value;
			}
		}

		/// DpnBasePointer calls this when it is created.
		/// If a pointer hasn't already been assigned, it
		/// will assign the newly created one by default.
		///
		/// This simplifies the common case of having only one
		/// IDpnPointer so is can be automatically hooked up
		/// to the manager.  If multiple DpnGazePointers are in
		/// the scene, the app has to take responsibility for
		/// setting which one is active.
		public static void OnPointerCreated(IDpnPointer createdPointer)
		{
			if (instance != null)
			{
				DpnPointerManager.Pointer = createdPointer;
			}
		}

        public static void OnPointerDestroy(IDpnPointer createdPointer)
        {
            if (instance != null && DpnPointerManager.Pointer == createdPointer)
            {
                DpnPointerManager.Pointer = null;
            }
        }

        private IDpnPointer pointer;

		void Awake()
		{
			if (instance != null)
			{
				Debug.LogError("More than one DpnPointerManager instance was found in your scene. "
				  + "Ensure that there is only one DpnPointerManager.");
				this.enabled = false;
				return;
			}

			instance = this;
		}

		void OnDestroy()
		{
			if (instance == this)
			{
				instance = null;
			}
		}

        static Vector2 GetPointerScreenPosition()
        {
            return instance != null ? Vector2.zero : Pointer.GetScreenPosition();
        }

        static public Vector3 pointerLocalPosition = new Vector3(0, 0, 2);
	}
}
