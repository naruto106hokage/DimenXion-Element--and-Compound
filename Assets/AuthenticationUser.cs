using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationUser : MonoBehaviour
{
    void Start()
    {
        DynamicGI.UpdateEnvironment();
        if (!PlayerPrefs.HasKey("DeviceInfo"))
        {
            PlayerPrefs.SetString("DeviceInfo", "DeviceInfoRegisterd");
            PlayerPrefs.SetString("DEVICE_ID", SystemInfo.deviceUniqueIdentifier);

        }
        if (PlayerPrefs.GetString("DEVICE_ID") == SystemInfo.deviceUniqueIdentifier)
        {
            Debug.Log("Launched");
        }
        else
        {
            if (PlayPauseSimulation.instance != null)
            {
                PlayPauseSimulation.instance.ShowStartErrorMessage(":(\n\nContact Support");
                Debug.Log("Not Launched");
            }

        }

    }


}


