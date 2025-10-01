/************************************************************************************

Copyright   :   Copyright 2015-2017 DeePoon LLC. All Rights reserved.

DPVR Developer Website: http://developer.dpvr.cn

************************************************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace dpn
{
	/// Represents the controller's current connection state.
	public enum DpnConnectionState
	{
		/// Indicates that an error has occurred.
		Error = -1,

		/// Indicates that the controller is disconnected.
		Disconnected = 0,
		/// Indicates that the device is scanning for controllers.
		Scanning = 1,
		/// Indicates that the device is connecting to a controller.
		Connecting = 2,
		/// Indicates that the device is connected to a controller.
		Connected = 3,

		Bond = 4,

		Unbond = 5,
	};

	// Represents the API status of the current controller state.
	public enum DpnControllerApiStatus
	{
		// A Unity-localized error occurred.
		Error = -1,

		// API is happy and healthy. This doesn't mean the controller itself
		// is connected, it just means that the underlying service is working
		// properly.
		Ok = 0,

		/// Any other status represents a permanent failure that requires
		/// external action to fix:

		/// API failed because this device does not support controllers (API is too
		/// low, or other required feature not present).
		Unsupported = 1,
		/// This app was not authorized to use the service (e.g., missing permissions,
		/// the app is blacklisted by the underlying service, etc).
		NotAuthorized = 2,
		/// The underlying VR service is not present.
		Unavailable = 3,
		/// The underlying VR service is too old, needs upgrade.
		ApiServiceObsolete = 4,
		/// The underlying VR service is too new, is incompatible with current client.
		ApiClientObsolete = 5,
		/// The underlying VR service is malfunctioning. Try again later.
		ApiMalfunction = 6,
	};

	/// Main entry point for the Daydream controller API.
	/// To use this API, add this behavior to a GameObject in your scene, or use the
	/// DpnControllerMain prefab. There can only be one object with this behavior on your scene.
	/// This is a singleton object.
	/// To access the controller state, simply read the static properties of this class. For example,
	/// to know the controller's current orientation, use DpnController.Orientation.
	public class DpnDaydreamController : DpnBasePeripheral
	{
		private static DpnDaydreamController instance = null;

        [Tooltip("If not set, relative to parent")]
        public Transform origin;

        public Transform model;

        public Transform reticlePointer;

		private DpnDaydreamControllerState controllerState = new DpnDaydreamControllerState();
		private DpnDaydreamControllerState lastcontrollerState = new DpnDaydreamControllerState();

        private Transform Pointer;

        static private Vector3 controllerLocalPosition = new Vector3(0.0f, 0.0019f, -0.00365f);

        private Vector2 _touchUpPos;


		[SerializeField]
		private Dpn3DofArmModel armModel;


        /// Returns the controller's current connection state.
        public static DpnConnectionState State
		{
			get
			{
				return instance != null ? instance.controllerState.connectionState : DpnConnectionState.Error;
			}
		}

		/// Returns the API status of the current controller state.
		public static DpnControllerApiStatus ApiStatus
		{
			get
			{
				return instance != null ? instance.controllerState.apiStatus : DpnControllerApiStatus.Error;
			}
		}

		/// Returns the controller's current orientation in space, as a quaternion.
		/// The space in which the orientation is represented is the usual Unity space, with
		/// X pointing to the right, Y pointing up and Z pointing forward. Therefore, to make an
		/// object in your scene have the same orientation as the controller, simply assign this
		/// quaternion to the GameObject's transform.rotation.
		public static Quaternion Orientation
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.orientation : Quaternion.identity;
			}
		}

		/// Returns the controller's gyroscope reading. The gyroscope indicates the angular
		/// about each of its local axes. The controller's axes are: X points to the right,
		/// Y points perpendicularly up from the controller's top surface and Z lies
		/// along the controller's body, pointing towards the front. The angular speed is given
		/// in radians per second, using the right-hand rule (positive means a right-hand rotation
		/// about the given axis).
		public static Vector3 Gyro
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.gyro : Vector3.zero;
			}
		}

		/// Returns the controller's accelerometer reading. The accelerometer indicates the
		/// effect of acceleration and gravity in the direction of each of the controller's local
		/// axes. The controller's local axes are: X points to the right, Y points perpendicularly
		/// up from the controller's top surface and Z lies along the controller's body, pointing
		/// towards the front. The acceleration is measured in meters per second squared. Note that
		/// gravity is combined with acceleration, so when the controller is resting on a table top,
		/// it will measure an acceleration of 9.8 m/s^2 on the Y axis. The accelerometer reading
		/// will be zero on all three axes only if the controller is in free fall, or if the user
		/// is in a zero gravity environment like a space station.
		public static Vector3 Accel
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.accel : Vector3.zero;
			}
		}

		/// If true, the user is currently touching the controller's touchpad.
		public static bool IsTouching
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.isTouching : false;
			}
		}

		/// If true, the user just started touching the touchpad. This is an event flag (it is true
		/// for only one frame after the event happens, then reverts to false).
		public static bool TouchDown
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.touchDown : false;
			}
		}

		/// If true, the user just stopped touching the touchpad. This is an event flag (it is true
		/// for only one frame after the event happens, then reverts to false).
		public static bool TouchUp
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.touchUp : false;
			}
		}

		public static Vector2 TouchPos
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.touchPos : Vector2.zero;
			}
		}

        // get touch up position 
        public static Vector2 TouchUpPos
        {
            get
            {
                return instance != null && instance.isValid ? instance._touchUpPos : Vector2.zero;
            }
        }

		public static bool RecenterBegin
		{
			get
			{
				return instance && instance.isValid && instance.controllerState.recenterBegin;
			}
		}
		/// If true, the user is currently performing the recentering gesture. Most apps will want
		/// to pause the interaction while this remains true.
		public static bool Recentering
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.recentering : false;
			}
		}

		/// If true, the user just completed the recenter gesture. The controller's orientation is
		/// now being reported in the new recentered coordinate system (the controller's orientation
		/// when recentering was completed was remapped to mean "forward"). This is an event flag
		/// (it is true for only one frame after the event happens, then reverts to false).
		/// The headset is recentered together with the controller.
		public static bool Recentered
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.recentered : false;
			}
		}

		public static bool RecenterCancel
		{
			get
			{
				return instance && instance.isValid && instance.controllerState.recenterCancel;
			}
		}

		/// If true, the click button (touchpad button) is currently being pressed. This is not
		/// an event: it represents the button's state (it remains true while the button is being
		/// pressed).
		public static bool ClickButton
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.clickButtonState : false;
			}
		}

		/// If true, the click button (touchpad button) was just pressed. This is an event flag:
		/// it will be true for only one frame after the event happens.
		public static bool ClickButtonDown
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.clickButtonDown : false;
			}
		}

		/// If true, the click button (touchpad button) was just released. This is an event flag:
		/// it will be true for only one frame after the event happens.
		public static bool ClickButtonUp
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.clickButtonUp : false;
			}
		}

        /*
		/// If true, the app button (touchpad button) is currently being pressed. This is not
		/// an event: it represents the button's state (it remains true while the button is being
		/// pressed).
		public static bool AppButton
		{
			get
			{
				return instance != null ? instance.controllerState.appButtonState : false;
			}
		}

		/// If true, the app button was just pressed. This is an event flag: it will be true for
		/// only one frame after the event happens.
		public static bool AppButtonDown
		{
			get
			{
				return instance != null ? instance.controllerState.appButtonDown : false;
			}
		}

		/// If true, the app button was just released. This is an event flag: it will be true for
		/// only one frame after the event happens.
		public static bool AppButtonUp
		{
			get
			{
				return instance != null ? instance.controllerState.appButtonUp : false;
			}
		}
        */

        /// If true, the trigger button  is currently being pressed. This is not an event: 
        /// it represents the button's state (it remains true while the button is being pressed).
        /// It is not supported on normal daydream controller.
        public static bool TriggerButton
        {
            get
            {
                return instance != null && instance.isValid ? instance.controllerState.triggerButtonState : false;
            }
        }

        /// If true, the trigger button was just pressed. This is an event flag: it will be true for
        /// only one frame after the event happens.
        public static bool TriggerButtonDown
        {
            get
            {
                return instance != null && instance.isValid ? instance.controllerState.triggerButtonDown : false;
            }
        }

        /// If true, the trigger button was just released. This is an event flag: it will be true for
        /// only one frame after the event happens.
        public static bool TriggerButtonUp
        {
            get
            {
                return instance != null && instance.isValid ? instance.controllerState.triggerButtonUp : false;
            }
        }

		/// If true, the volumeUp button (touchpad button) is currently being pressed. This is not
		/// an event: it represents the button's state (it remains true while the button is being
		/// pressed).
		public static bool volumeUpButton
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.volumeUpButtonState : false;
			}
		}

		/// If true, the volumeUp button was just pressed. This is an event flag: it will be true for
		/// only one frame after the event happens.
		public static bool volumeUpButtonDown
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.volumeUpButtonDown : false;
			}
		}

		/// If true, the volumeUp button was just released. This is an event flag: it will be true for
		/// only one frame after the event happens.
		public static bool volumeUpButtonUp
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.volumeUpButtonUp : false;
			}
		}

		/// If true, the volumeDown button (touchpad button) is currently being pressed. This is not
		/// an event: it represents the button's state (it remains true while the button is being
		/// pressed).
		public static bool volumeDownButton
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.volumeDownButtonState : false;
			}
		}

		/// If true, the volumeDown button was just pressed. This is an event flag: it will be true for
		/// only one frame after the event happens.
		public static bool volumeDownButtonDown
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.volumeDownButtonDown : false;
			}
		}

		/// If true, the volumeDown button was just released. This is an event flag: it will be true for
		/// only one frame after the event happens.
		public static bool volumeDownButtonUp
		{
			get
			{
				return instance != null && instance.isValid ? instance.controllerState.volumeDownButtonUp : false;
			}
		}

		public static int BatteryPower
		{
			get
			{
				if (instance == null)
					return 100;
				else
					return instance.controllerState.batteryPower;
			}
		}

		/// If State == DpnConnectionState.Error, this contains details about the error.
		public static string ErrorDetails
		{
			get
			{
				if (instance != null)
				{
					return instance.controllerState.connectionState == DpnConnectionState.Error ?
						instance.controllerState.errorDetails : "";
				}
				else
				{
					return "DpnController instance not found in scene. It may be missing, or it might "
						+ "not have initialized yet.";
				}
			}
		}

        public void OnEnable()
		{
			if (instance != null)
			{
				Debug.LogError("More than one DpnController instance was found in your scene. "
					+ "Ensure that there is only one DpnController.");
				instance.enabled = false;
			}
            //string deviceName = "WiseVision_DayDream_Controller";
            string deviceName = null;
            if (DpnManager.peripheral == DPVRPeripheral.Flip)
            {
                deviceName = "SkyWorth_DayDream_Controller";
            }
            if (!OpenPeripheral(deviceName))
            {
                Debug.Log("Open Peripheral " + deviceName + " fails.");
                return;
            }

            instance = this;

            if (DpnManager.DPVRPointer)
            {

                // Update by Umety
                Pointer = Instantiate(reticlePointer, this.transform, false);
                //Pointer.SetParent(this.transform, false);
                //============================
                Pointer.gameObject.SetActive(false);
                DpnBasePointer pointer = Pointer.GetComponent<DpnBasePointer>();
                if(pointer)
                {
                    pointer.SetTitledAngle(-15.0f);
                }
            }

        }

        public override void OnDisable()
		{
            base.OnDisable();
            if (instance == this)
            {
                instance = null;
            }
		}

		public override void DpnpUpdate()
		{
            int fff = peripheral.DpnupReadDeviceAttribute((int)DPNP_DAYDREAM_ATTRIBUTE.DPNP_DAYDREAM_ATTRIBUTE_UPDATE, IntPtr.Zero, 0);
            if (1 != fff)
            {
                Debug.Log("Deepoon: Controller update fails");
            }

            base.DpnpUpdate();

            ReadState(controllerState, lastcontrollerState);

            UpdateGesturePos();

            if(controllerState.connectionState != lastcontrollerState.connectionState)
            {
                switch(controllerState.connectionState)
                {
                    case DpnConnectionState.Error:
                        break;
                    case DpnConnectionState.Disconnected:
                        OnDisconnected();
                        break;
                    case DpnConnectionState.Connected:
                        OnConnected();
                        break;
                }
            }
            // If a headset recenter was requested, do it now.
            if (controllerState.headsetRecenterRequested)
			{
				DpnCameraRig._instance.Recenter();
			}

            lastcontrollerState.CopyFrom(controllerState);

            //i3vr.I3vrArmModel.Instance.OnControllerUpdate();

            //UpdatePoses();
		}

//        void UpdatePoses()
//        {
//            int device_state = DpnpGetDeviceCurrentStatus().device_status;
//
//            if (device_state != (int)DpnConnectionState.Connected)
//            {
//                return;
//            }
//
//            Vector3 pos = i3vr.I3vrArmModel.Instance.wristPosition * DpnManager.worldScale + controllerLocalPosition;
//            Quaternion rot = i3vr.I3vrArmModel.Instance.wristRotation;
//
//            if (origin != null)
//            {
//                transform.position = origin.transform.TransformPoint(pos);
//                transform.rotation = origin.rotation * rot;
//            }
//            else
//            {
//                transform.localPosition = pos;
//                transform.localRotation = rot;
//            }
//        }

        int GetPatteryPower()
        {
            IntPtr temp = Marshal.AllocHGlobal(sizeof(int));
            peripheral.DpnupReadDeviceAttribute(DPNP_VALUE_TYPE.DPNP_VALUE_TYPE_ATTRIBUTE_BATTERY_POWER - DPNP_VALUE_TYPE.DPNP_VALUE_TYPE_ATTRIBUTE, temp, sizeof(int));
            int power = Marshal.ReadInt32(temp);
            Marshal.FreeHGlobal(temp);
            return power;
        }

        void ReadState (DpnDaydreamControllerState outState, DpnDaydreamControllerState lastState)
        {
            outState.connectionState = (DpnConnectionState)DpnpGetDeviceCurrentStatus().device_status;
            outState.apiStatus = DpnControllerApiStatus.Ok;

            float[] pose = DpnpGetDeviceCurrentStatus().pose_state[0];
            dpnQuarterion rawOri = new dpnQuarterion { s = pose[0], i = pose[1], j = pose[2], k = pose[3] };
            dpnVector3 rawAccel = new dpnVector3 { x = pose[7], y = pose[8], z = pose[9] };
            dpnVector3 rawGyro = new dpnVector3 { x = pose[4], y = pose[5], z = pose[6] };
            outState.orientation = rawOri.ToQuaternion();
            //outState.orientation = new Quaternion(pose[1], pose[2], pose[3], pose[0]);
            outState.accel = new Vector3(rawAccel.x, rawAccel.y, -rawAccel.z);
            outState.gyro = new Vector3(-rawGyro.x, -rawGyro.y, rawGyro.z);

            outState.batteryPower = GetPatteryPower();
            switch (DpnManager.peripheral)
            {
                case DPVRPeripheral.Flip:
                {
                    float touchPos_x = DpnpGetDeviceCurrentStatus().axis_state[(int)DPNP_DAYDREAM_AXES.DPNP_DAYDREAM_AXIS_X][0];
                    float touchPos_y = DpnpGetDeviceCurrentStatus().axis_state[(int)DPNP_DAYDREAM_AXES.DPNP_DAYDREAM_AXIS_Y][0];

                    switch (DpnManager.touchPosOrig)
                    {
                        case TouchPosOrig.TOP_LEFT:
                            outState.touchPos = new Vector2(touchPos_x, touchPos_y);
                            break;
                        case TouchPosOrig.CENTER:
                            outState.touchPos = new Vector2(touchPos_x, touchPos_y) * 2.0f - Vector2.one;
                            break;
                    }

                    outState.isTouching = 0 != DpnpGetDeviceCurrentStatus().button_state[(int)DPNP_FLIP_BUTTONS.DPNP_FLIP_BUTTON_TOUCH][0];

                    outState.touchDown = !lastState.isTouching && outState.isTouching;
                    outState.touchUp = lastState.isTouching && !outState.isTouching;

                    outState.appButtonState = 0 != DpnpGetDeviceCurrentStatus().button_state[(int)DPNP_FLIP_BUTTONS.DPNP_FLIP_BUTTON_APP][0];
                    outState.appButtonDown = !lastState.appButtonState && outState.appButtonState;
                    outState.appButtonUp = lastState.appButtonState && !outState.appButtonState;

                    outState.clickButtonState = 0 != DpnpGetDeviceCurrentStatus().button_state[(int)DPNP_FLIP_BUTTONS.DPNP_FLIP_BUTTON_CLICK][0];
                    outState.clickButtonDown = !lastState.clickButtonState && outState.clickButtonState;
                    outState.clickButtonUp = lastState.clickButtonState && !outState.clickButtonState;

                    outState.triggerButtonState = 0 != DpnpGetDeviceCurrentStatus().button_state[(int)DPNP_FLIP_BUTTONS.DPNP_FLIP_BUTTON_TRIGGGER][0];
                    outState.triggerButtonDown = !lastState.triggerButtonState && outState.triggerButtonState;
                    outState.triggerButtonUp = lastState.triggerButtonState && !outState.triggerButtonState;

                    outState.volumeUpButtonState = false;
                    outState.volumeUpButtonDown = false;
                    outState.volumeUpButtonUp = false;

                    outState.volumeDownButtonState = false;
                    outState.volumeDownButtonDown = false;
                    outState.volumeDownButtonUp = false;

                    outState.recentering = 0 != DpnpGetDeviceCurrentStatus().button_state[(int)DPNP_FLIP_BUTTONS.DPNP_FLIP_BUTTON_RECENTERING][0];
                    outState.recentered = 0 != DpnpGetDeviceCurrentStatus().button_state[(int)DPNP_FLIP_BUTTONS.DPNP_FLIP_BUTTON_RECENTERED][0];
		      outState.recenterBegin = controllerState.recentering && !lastcontrollerState.recentering;
                    outState.recenterCancel = !controllerState.recentered && !controllerState.recentering && lastcontrollerState.recentering;

                    //Debug.Log(" DpnDaydreamControllerState GetData:" + "connectionState = " + outState.connectionState
                    //+ "  outState.accX = " + outState.accel.x + "   outState.accY = " + outState.accel.y + "  outState.accZ = " + outState.accel.z
                    //+ "  outState.gyroX = " + outState.gyro.x + "   outState.gyroY = " + outState.gyro.y + "  outState.gyroZ = " + outState.gyro.z
                    //+ "  outState.oriX = " + outState.orientation.x + "   outState.oriY = " + outState.orientation.y + "  outState.oriZ = " + outState.orientation.z
                    //+ "  outState.touchX = " + outState.touchPos.x + "   outState.touchY = " + outState.touchPos.y
                    //+ "  outState.btnAPP = " + outState.appButtonState + "   outState.btnHome = " + outState.recentering + "   outState.btnClick = " + outState.clickButtonState
                    //+ "  outState.btnvolumeUp = " + outState.volumeUpButtonState + "   outState.btnvolumeDown = " + outState.volumeDownButtonState);

                    // If the controller was recentered, we may also need to request that the headset be
                    // recentered. We should do that only if VrCore does NOT implement recentering.
                    outState.headsetRecenterRequested = outState.recentered;
                    if (outState.touchUp)
                    {
                        _touchUpPos = lastcontrollerState.touchPos;
                    }
                    break;
                }
                default:
                {
                    float touchPos_x = DpnpGetDeviceCurrentStatus().axis_state[(int)DPNP_DAYDREAM_AXES.DPNP_DAYDREAM_AXIS_X][0];
                    float touchPos_y = DpnpGetDeviceCurrentStatus().axis_state[(int)DPNP_DAYDREAM_AXES.DPNP_DAYDREAM_AXIS_Y][0];
                    outState.touchPos = new Vector2(touchPos_x, touchPos_y);
                    switch (DpnManager.touchPosOrig)
                    {
                        case TouchPosOrig.TOP_LEFT:
                            outState.touchPos = new Vector2(touchPos_x, touchPos_y);
                            outState.isTouching = (0 != touchPos_x) && (0 != touchPos_y);
                            break;
                        case TouchPosOrig.CENTER:
                            outState.touchPos = new Vector2(touchPos_x, touchPos_y) * 2.0f - Vector2.one;
                            outState.isTouching = (-1 != touchPos_x) && (-1 != touchPos_y);
                            break;
                    }

                    outState.touchDown = !lastState.isTouching && outState.isTouching;
                    outState.touchUp = lastState.isTouching && !outState.isTouching;

                    outState.appButtonState = 0 != DpnpGetDeviceCurrentStatus().button_state[(int)DPNP_DAYDREAM_BUTTONS.DPNP_DAYDREAM_BUTTON_APP][0];
                    outState.appButtonDown = !lastState.appButtonState && outState.appButtonState;
                    outState.appButtonUp = lastState.appButtonState && !outState.appButtonState;

                    outState.clickButtonState = 0 != DpnpGetDeviceCurrentStatus().button_state[(int)DPNP_DAYDREAM_BUTTONS.DPNP_DAYDREAM_BUTTON_CLICK][0];
                    outState.clickButtonDown = !lastState.clickButtonState && outState.clickButtonState;
                    outState.clickButtonUp = lastState.clickButtonState && !outState.clickButtonState;

                    outState.triggerButtonState = false;
                    outState.triggerButtonDown = false;
                    outState.triggerButtonUp = false;

                    outState.volumeUpButtonState = 0 != DpnpGetDeviceCurrentStatus().button_state[(int)DPNP_DAYDREAM_BUTTONS.DPNP_DAYDREAM_BUTTON_VOLUMEUP][0];
                    outState.volumeUpButtonDown = !lastState.volumeUpButtonState && outState.volumeUpButtonState;
                    outState.volumeUpButtonUp = lastState.volumeUpButtonState && !outState.volumeUpButtonState;

                    outState.volumeDownButtonState = 0 != DpnpGetDeviceCurrentStatus().button_state[(int)DPNP_DAYDREAM_BUTTONS.DPNP_DAYDREAM_BUTTON_VOLUMEDOWN][0];
                    outState.volumeDownButtonDown = !lastState.volumeDownButtonState && outState.volumeDownButtonState;
                    outState.volumeDownButtonUp = lastState.volumeDownButtonState && !outState.volumeDownButtonState;

                    outState.recentering = 0 != DpnpGetDeviceCurrentStatus().button_state[(int)DPNP_DAYDREAM_BUTTONS.DPNP_DAYDREAM_BUTTON_HOME][0];
                    outState.recentered = lastState.recentering && !outState.recentering;
		      outState.recenterBegin = controllerState.recentering && !lastcontrollerState.recentering;
                    outState.recenterCancel = !controllerState.recentered && !controllerState.recentering && lastcontrollerState.recentering;

                    //Debug.Log(" DpnDaydreamControllerState GetData:" + "connectionState = " + outState.connectionState
                    //+ "  outState.accX = " + outState.accel.x + "   outState.accY = " + outState.accel.y + "  outState.accZ = " + outState.accel.z
                    //+ "  outState.gyroX = " + outState.gyro.x + "   outState.gyroY = " + outState.gyro.y + "  outState.gyroZ = " + outState.gyro.z
                    //+ "  outState.oriX = " + outState.orientation.x + "   outState.oriY = " + outState.orientation.y + "  outState.oriZ = " + outState.orientation.z
                    //+ "  outState.touchX = " + outState.touchPos.x + "   outState.touchY = " + outState.touchPos.y
                    //+ "  outState.btnAPP = " + outState.appButtonState + "   outState.btnHome = " + outState.recentering + "   outState.btnClick = " + outState.clickButtonState
                    //+ "  outState.btnvolumeUp = " + outState.volumeUpButtonState + "   outState.btnvolumeDown = " + outState.volumeDownButtonState);

                    // If the controller was recentered, we may also need to request that the headset be
                    // recentered. We should do that only if VrCore does NOT implement recentering.
                    outState.headsetRecenterRequested = outState.recentered;
                    if (outState.touchUp)
                    {
                        _touchUpPos = lastcontrollerState.touchPos;
                    }
                    break;
                }
            }
        }

        public override void DpnpResume()
        {
            peripheral.DpnupResume();
            CheckConnectState();

            StartCoroutine(Coroutine_UpdateInteractiveHand());
        }

        void Start()
        {
            if (peripheral == null)
            {
                OnDisconnected();
                this.gameObject.SetActive(false);
                return;
            }
            CheckConnectState();

            UpdateInteractiveHand();

            onUnbind += OnUnbind;
        }
        //triggered by OnApplicationPause and OnApplicationFocus
        public override void DpnpPause()
        {
            peripheral.DpnupPause();
        }

        void OnDisconnected()
        {
            _isValid = false;
            SendMessageUpwards("OnPeripheralDisconnected", this);
        }
        void OnConnected()
        {
            _isValid = true;
            SendMessageUpwards("OnPeripheralConnected", this);
            _interactiveHand = GetInteractiveHand();

            if(onConnected != null)
                onConnected(this);
        }

        public override void EnableInternalObjects(bool enabled)
        {
            if(model && model.gameObject)
                model.gameObject.SetActive(enabled);

            if (DpnManager.DPVRPointer)
            {

                if (Pointer && Pointer.gameObject)
                    Pointer.gameObject.SetActive(enabled);

                if (Pointer && enabled)
                    DpnPointerManager.Pointer = (IDpnPointer)Pointer.GetComponent<ReticlePointer>();

                UpdateInteractiveHand();

                if (Pointer)
                {
                    if (enabled)
                    {
                        Pointer.localPosition = DpnPointerManager.pointerLocalPosition;
                    }
                    else
                    {
                        DpnPointerManager.pointerLocalPosition = Pointer.localPosition;
                    }
                }
            }
        }

        public override bool BeingUsed()
        {
            return DpnDaydreamController.IsTouching
                || DpnDaydreamController.TriggerButtonDown || DpnDaydreamController.TriggerButtonUp
                || DpnDaydreamController.RecenterBegin || DpnDaydreamController.Recentering
                || DpnDaydreamController.BackButtonDown || DpnDaydreamController.BackButtonUp;
        }

        void CheckConnectState()
        {
            ReadState(controllerState, lastcontrollerState);
            if (controllerState.connectionState != DpnConnectionState.Connected)
            {
                OnDisconnected();
            }
        }

        Vector2 _gestureBeginPos;
        Vector2 _gestureEndPos;

        void UpdateGesturePos()
        {
            if (controllerState.touchDown)
            {
                _gestureBeginPos = controllerState.touchPos;
            }
            else if (controllerState.touchUp)
            {
                _gestureEndPos = lastcontrollerState.touchPos;
            }
        }

        static public bool TouchGestureDown
        {
            get
            {
                if (instance != null && instance._isValid && instance.controllerState.touchUp)
                {
                    Vector2 delta = instance._gestureEndPos - instance._gestureBeginPos;
                    return (delta.y > 0.3f) && (Mathf.Abs(delta.y) > Mathf.Abs(delta.x));
                }
                return false;
            }
        }

        static public bool TouchGestureUp
        {
            get
            {
                if (instance != null && instance._isValid && instance.controllerState.touchUp)
                {
                    Vector2 delta = instance._gestureEndPos - instance._gestureBeginPos;
                    return (delta.y < -0.3f) && (Mathf.Abs(delta.y) > Mathf.Abs(delta.x));
                }
                return false;
            }

        }

        static public bool TouchGestureLeft
        {
            get
            {
                if (instance != null && instance._isValid && instance.controllerState.touchUp)
                {
                    Vector2 delta = instance._gestureEndPos - instance._gestureBeginPos;
                    return (delta.x < -0.3f) && (Mathf.Abs(delta.x) > Mathf.Abs(delta.y));
                }
                return false;
            }
        }

        static public bool TouchGestureRight
        {
            get
            {
                if (instance != null && instance._isValid && instance.controllerState.touchUp)
                {
                    Vector2 delta = instance._gestureEndPos - instance._gestureBeginPos;
                    return (delta.x > 0.3f) && (Mathf.Abs(delta.x) > Mathf.Abs(delta.y));
                }
                return false;
            }
        }

        static public bool BackButton
        {
            get
            {
                return instance != null 
                    && instance._isValid
                    && instance.controllerState.appButtonState;
            }
        }

        static public bool BackButtonDown
        {
            get
            {
                return instance != null 
                    && instance._isValid
                    && instance.controllerState.appButtonDown;
            }
        }

        static public bool BackButtonUp
        {
            get
            {
                return instance != null 
                    && instance._isValid
                    && instance.controllerState.appButtonUp;
            }
        }

        private int _interactiveHand = 0;

        void SetInteractiveHand(int interactiveHand)
        {
            if (peripheral == null)
                return;

            if (interactiveHand != 0 && interactiveHand != 1)
            {
                Debug.LogError("SetInteractiveHand : interactiveHand is invalid value");
                return;
            }

            _interactiveHand = interactiveHand;

            IntPtr buffer = Marshal.AllocHGlobal(sizeof(int));
            int[] values = new int[1];
            values[0] = interactiveHand;
            Marshal.Copy(values, 0, buffer, 1);

            peripheral.DpnupSetDeviceAttribute(DPNP_VALUE_TYPE.DPNP_VALUE_TYPE_ATTRIBUTE_INTERACTIVE_HAND - DPNP_VALUE_TYPE.DPNP_VALUE_TYPE_ATTRIBUTE
                , buffer, sizeof(int));

            Marshal.FreeHGlobal(buffer);

            UpdateArmModel();
        }

        int GetInteractiveHand()
        {
            if (peripheral == null)
                return 0;

            IntPtr buffer = Marshal.AllocHGlobal(sizeof(int));
            int ret = peripheral.DpnupReadDeviceAttribute(DPNP_VALUE_TYPE.DPNP_VALUE_TYPE_ATTRIBUTE_INTERACTIVE_HAND - DPNP_VALUE_TYPE.DPNP_VALUE_TYPE_ATTRIBUTE
            , buffer, sizeof(int));
            int interactiveHand = 0;
            if (ret == 1)
            {
                int[] value = new int[1];
                Marshal.Copy(buffer, value, 0, 1);
                interactiveHand = value[0];
            }
            Marshal.FreeHGlobal(buffer);
            return interactiveHand;
        }

        void UpdateArmModel()
        {
//            if(i3vr.I3vrArmModel.Instance != null)
//                i3vr.I3vrArmModel.Instance.interactiveHand = _interactiveHand;

			if (armModel != null)
				armModel.interactiveHand = _interactiveHand;
			
        }

        static public int interactiveHand
        {
            set
            {
                if (instance != null)
                    instance.SetInteractiveHand(value);
            }
            get
            {
                return instance != null ? instance.GetInteractiveHand() : 0; 
            }
        }

        IEnumerator Coroutine_UpdateInteractiveHand()
        {
            yield return new WaitForSeconds(0.5f);
            UpdateInteractiveHand();
        }

        void UpdateInteractiveHand()
        {
            _interactiveHand = GetInteractiveHand();
            UpdateArmModel();
        }

        public static void StartBootPair()
        {
            instance.peripheral.DpnupBootPair(true);
        }

        public static void StopBootPair()
        {
            instance.peripheral.DpnupBootPair(false);
        }

        void OnUnbind(DpnBasePeripheral peripheral)
        {
            if (peripheral != this)
                return;

            _isValid = false;
        }

        public static void Unbind()
        {
            instance.peripheral.DpnupSetUnbindDevice();
            onUnbind(instance);
        }

        public delegate void Delegate_OnConnected(DpnBasePeripheral peripheral);
        public delegate void Delegate_OnUnbind(DpnBasePeripheral peripheral);

        public static Delegate_OnConnected onConnected = null;

        public static Delegate_OnConnected onUnbind = null;

        public static string GetBondDeviceName()
        {
            return instance.peripheral.DpnupGetBondDeviceName();
        }

    }
}
