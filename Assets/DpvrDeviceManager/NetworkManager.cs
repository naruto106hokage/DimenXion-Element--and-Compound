using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dpvr
{
    public partial class DpvrDeviceManager
    {
        public class NetworkManager
        {
            private static OnNetworkCallback networkCallback;

            public static void OpenAndroidWifiSetting()
            {
                GetDeviceManager().Call("openAndroidWifiSetting");
            }

            public static void RegisterNetworkCallback(OnNetReachableHandler reachableHandler,
                                                       OnWifiExtrasHandler extrasHandler,
                                                       int levelCount = 5)
            {
                networkCallback = new OnNetworkCallback(reachableHandler, extrasHandler);
                GetDeviceManager().Call("registerNetworkCallback", networkCallback, levelCount);
            }

            public static bool IsNetworkAvailable()
            {
                return GetDeviceManager().Call<bool>("isNetworkAvailable");
            }

            public static int GetWifiStrength()
            {
                return GetDeviceManager().Call<int>("getWifiStrength");
            }

            /**
             * tips:
             * mac address as a stable value, recommend user to save the value into local
             * file instead of getting mac address everywhere
             */
            public static string GetMacAddress()
            {
                return GetDeviceManager().Call<string>("getMacAddress");
            }

            public delegate void OnNetReachableHandler(bool reachable);
            public delegate void OnWifiExtrasHandler(string ssid, int signalLevel, int speed, string units);
            class OnNetworkCallback : AndroidJavaProxy
            {
                OnNetReachableHandler netReachableHandler;
                OnWifiExtrasHandler netExtrasHandler;
                public OnNetworkCallback(OnNetReachableHandler netReachable, OnWifiExtrasHandler netExtras)
                    : base("com.dpvr.DeviceManager.PluginCallback$NetworkCallback")
                {
                    this.netExtrasHandler = netExtras;
                    this.netReachableHandler = netReachable;
                }

                public void OnNetworkReachable(bool reachable)
                {
                    if (netReachableHandler != null)
                    {
                        netReachableHandler(reachable);
                    }
                }

                public void OnNetworkExtras(string ssid, int signalLevel,
                                     int speed, string units)
                {
                    if (netExtrasHandler != null)
                    {
                        netExtrasHandler(ssid, signalLevel, speed, units);
                    }
                }
            }
        }
    }
}
