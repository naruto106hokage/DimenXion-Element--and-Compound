#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Build;
using System.IO;
using UnityEngine.VR;
using UnityEditorInternal.VR;

class PreBuild : IPreprocessBuild
{
    public int callbackOrder { get { return 0; } }

    const string assetPathPicoDev = "Assets/AdvancementIntegration/Wrapper/DevelopmentAndroidManifest.xml";
    const string assetPathPicoRelease = "Assets/AdvancementIntegration/Wrapper/ReleaseAndroidManifest.xml";
    const string assetPathManifest = "Assets/Plugins/Android/AndroidManifest.xml";

    //Gets called before build is executed]
    public void OnPreprocessBuild(BuildTarget target, string path)
    {
        CheckDevelopment();
    }

    void CheckDevelopment()
    {
        AssetDatabase.DeleteAsset("Assets/Plugins/Android/AndroidManifest.xml");
        if (EditorUserBuildSettings.development)
        {
            AssetDatabase.CopyAsset(assetPathPicoDev, assetPathManifest);
        }
        else
        {
            AssetDatabase.CopyAsset(assetPathPicoRelease, assetPathManifest);
        }
    }
}
#endif