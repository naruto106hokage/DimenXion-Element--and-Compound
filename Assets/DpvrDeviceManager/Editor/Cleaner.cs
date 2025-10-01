using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Cleaner : ScriptableWizard
{
    static Cleaner()
    {
        string srcFile = "Assets/Plugins/Android/libDPVRDeviceManager.jar";
        if(File.Exists(srcFile))
            FileUtil.DeleteFileOrDirectory(srcFile);
        //Debug.Log("Haha, running when load");
    }
}
