using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dpvr
{
    public partial class DpvrDeviceManager
    {
        public class BluetoothManager
        {
            public const int bluetoothStateOff = 10;
            public const int bluetoothStateTurningOn = 11;
            public const int bluetoothStateOn = 12;
            public const int bluetoothStateTurningOff = 13;

            public const int deviceBondNone = 10;
            public const int deviceBonding = 11;
            public const int deviceBonded = 12;

            private static BluetoothCallback bluetoothCallback;

            public static bool IsBluetoothEnabled()
            {
                return GetDeviceManager().Call<bool>("isBluetoothEnabled");
            }

            public static void OpenAndroidBluetoothSetting()
            {
                GetDeviceManager().Call("openAndroidBluetoothSetting");
            }

            public static void RegisterBluetoothCallback(OnBtAdapterCallback btAdapter,
                                                         OnBtDeviceCallback btDevice,
                                                         OnBtDeviceBondCallback btDeviceBond)
            {
                bluetoothCallback = new BluetoothCallback(btAdapter, btDevice, btDeviceBond);
                GetDeviceManager().Call("registerBluetoothReceiver", bluetoothCallback);
            }

            // local bluetooth on/off state
            public delegate void OnBtAdapterCallback(int state);
            // device connection state
            public delegate void OnBtDeviceCallback(string deviceName, bool connected);
            // device bonding state
            public delegate void OnBtDeviceBondCallback(string deviceName, int bondState);
            class BluetoothCallback : AndroidJavaProxy
            {
                OnBtAdapterCallback btAdapterHandler;
                OnBtDeviceCallback btDeviceHandler;
                OnBtDeviceBondCallback btDeviceBondHandler;
                public BluetoothCallback(OnBtAdapterCallback btAdapter, OnBtDeviceCallback btDevice,
                    OnBtDeviceBondCallback btDeviceBond) : base("com.dpvr.DeviceManager.PluginCallback$BluetoothCallback")
                {
                    this.btAdapterHandler = btAdapter;
                    this.btDeviceHandler = btDevice;
                    this.btDeviceBondHandler = btDeviceBond;
                }

                void OnBtAdapterCallback(int state)
                {
                    if (btAdapterHandler != null)
                        btAdapterHandler(state);
                }
                void OnBtDeviceCallback(string name, bool connected)
                {
                    if (btDeviceHandler != null)
                        btDeviceHandler(name, connected);
                }
                void OnBtDeviceBondCallback(string name, int bondState)
                {
                    if (btDeviceBondHandler != null)
                        btDeviceBondHandler(name, bondState);
                }
            }
        }
    }
}
