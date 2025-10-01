﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dpvr
{
    public partial class DpvrDeviceManager
    {
        public class HomeAppManager
        {
            public static void SetHomeKeyApp(string packageName, string activityName)
            {
                GetDeviceManager().Call("setHomeKeyApp", packageName, activityName);
            }

            public static void RestoreHomeKeyApp()
            {
                GetDeviceManager().Call("restoreHomeKeyApp");
            }

            public static void SetNewLauncher(string packageName)
            {
                GetDeviceManager().Call("setNewLauncher", packageName);
            }

            public static void RestoreDpnLauncher()
            {
                GetDeviceManager().Call("restoreDpnLauncher");
            }
        }
    }
}
