/************************************************************************************

Copyright   :   Copyright 2015-2017 DeePoon LLC. All Rights reserved.

DPVR Developer Website: http://developer.dpvr.cn

************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Threading;

namespace dpn
{
    public class DpnBasePeripheral : MonoBehaviour
    {
        public DpnPeripheral peripheral = null;

        //protected Peripheralstatus peripheralstatus = null;

        //protected Peripheralstatus prevperipheralstatus = null;

        public virtual bool OpenPeripheral(DPNP_DEVICE_TYPE type, int index)
        {
            if (DpnDevice._instance == null)
            {
                DpnDevice.create();
            }
            #if UNITY_ANDROID && !UNITY_EDITOR
            if (type == DPNP_DEVICE_TYPE.DPNP_DEVICE_TYPE_HEAD_TRACKER)
            {
                peripheral = DpnDevice.OpenPeripheral(string.Empty, this);
                return true;
            }
            #endif

            int count = DpnPeripheral.DpnupQueryDeviceCount(type);
            if (count < 0 || index >= count)
            {
                return false;
            }
            string devicename = DpnPeripheral.DpnupGetDeviceId(type, index);
            if (devicename != null)
            {
                return OpenPeripheral(devicename);
            }
            return false;
        }

        public virtual bool OpenPeripheral(string deviceId)
        {
            if (DpnDevice._instance == null)
            {
                DpnDevice.create();
            }
#if UNITY_ANDROID && UNITY_EDITOR
            if (DpnManager.androidEditorUseHmd)
            {
                DpnPeripheral peripheral = DpnDevice.OpenPeripheral(deviceId, this);
                return peripheral != null;
            }
            else
            {
                peripheral = null;
                return true;
            }
#else
            DpnPeripheral peripheral = DpnDevice.OpenPeripheral(deviceId, this);
            return peripheral != null;
#endif

        }

        public virtual void OnDisable()
        {
            ClosePeripheral();
        }

        public void ClosePeripheral()
        {
            if (peripheral == null)
            {
                return;
            }
            DpnDevice.ClosePeripheral(this);
        }

        public PeripheralInfo DpnpGetDeviceInfo ()
        {
            if (peripheral == null)
            {
                return null;
            }
            return peripheral.peripheralInfo;
        }

        public Peripheralstatus DpnpGetDeviceCurrentStatus()
        {
            if (peripheral == null)
            {
                return null;
            }
            return peripheral.peripheralstatus;
        }

        public Peripheralstatus DpnpGetDevicePrevStatus()
        {
            if (peripheral == null)
            {
                return null;
            }
            return peripheral.prevperipheralstatus;
        }
        
        public virtual void DpnpUpdate()
        {
            if (peripheral == null)
            {
                return;
            }
            peripheral.DpnupUpdateDeviceState();
        }

        public virtual void DpnpPause()
        {
            if (peripheral == null)
            {
                return;
            }
            peripheral.DpnupPause();
        }

        public virtual void DpnpResume()
        {
            if (peripheral == null)
            {
                return;
            }
            peripheral.DpnupResume();
        }

        public override string ToString()
        {
            return peripheral._deviceId;
        }

        virtual public void EnableInternalObjects(bool enabled)
        {

        }

        virtual public bool BeingUsed()
        {
            return false;
        }

        virtual public void EnableModel(bool enabled)
        {

        }
        
        virtual public void EnablePointer(bool enabled)
        {

        }

        protected bool _isValid = true;

        public bool isValid
        {
            get
            {
                return _isValid;
            }
        }

    }
}
