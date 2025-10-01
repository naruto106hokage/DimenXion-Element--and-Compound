using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dpvr
{
    public partial class DpvrDeviceManager
    {
        public class SystemInfoManager
        {
            public static string GetProductModel()
            {
                return GetDeviceManager().Call<string>("getProductModel");
            }

            public static string GetLinuxVersion()
            {
                return GetDeviceManager().Call<string>("getLinuxVersion");
            }

            public static string GetAndroidVersion()
            {
                return GetDeviceManager().Call<string>("getAndroidVersion");
            }

            public static string GetBuildNumber()
            {
                return GetDeviceManager().Call<string>("getBuildNumber");
            }

            public static string GetSerialNo()
            {
                return GetDeviceManager().Call<string>("getSerialNo");
            }

            public static void OpenSystemSettings()
            {
                AppManager.GoToApp("com.android.settings");
            }
        }
    }
}
