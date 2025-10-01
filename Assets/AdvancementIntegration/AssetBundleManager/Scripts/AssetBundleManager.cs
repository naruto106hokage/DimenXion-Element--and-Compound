using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using UnityEngine.Networking;

public class AssetBundleManager : MonoBehaviour
{
	#region Variables
    static AssetBundle assetBundle;
    static string CurrActiveLanguageCode = "";
    const string fontName = "font";
	#endregion
	#region Creating Instance
    private static AssetBundleManager instance = null;
    public static AssetBundleManager Instance {
        get 
		{
            if (instance == null)
                instance = FindObjectOfType<AssetBundleManager>(); 
            if(CurrActiveLanguageCode == null)
             CurrActiveLanguageCode = PlayerPrefs.GetString("currentLanguage");
            return instance;
        }
    }
	#endregion

    #region GetThePath
    //Set the target path 
    // it is to save the file with the extention
    // Becoz of EasyBgDownloader we are unale to set android path from here 
    //But this method is usefull to get the bundle is exits in the path or not
    public string GetTheDestinationPath()
    {
        string trgtPath;
        string modifiedLangCode = PlayerPrefs.GetString("currentLanguage");
		//Debug.Log ("modifiedLangCode"+modifiedLangCode);
        trgtPath = Application.persistentDataPath;
		trgtPath = Path.Combine(trgtPath, ( GetTopicID() + "-" + modifiedLangCode.ToLower()) + ".unity3d");
		//Debug.Log ("TestingTargetPath"+trgtPath);
        return trgtPath;
    }
    #endregion

	#region GetTheTopicID
	internal string GetTopicID() {
		string[] TopicID = Application.identifier.Split ('.');
		if(TopicID.Length >= 3)
			return TopicID [3];
		Debug.LogError ("Topic ID is not correct in Package Name");
		return "";
	}

    #endregion
    #region GetFontName
    public string GetFontName()
    {
        string fullFontName = fontName+"-" + PlayerPrefs.GetString("currentLanguage");
        return fullFontName;
    }
    #endregion
    #region GettingAssetBundle
    public AssetBundle GetAssetBundle (string languageCode) 
    {
        languageCode = languageCode.ToLower();
        if (CurrActiveLanguageCode.Equals(languageCode) )
        {
			//assetBundle.Unload(true);
            if (assetBundle == null)
            {
               // Debug.Log("BundleIsUnloadAndNull");
                LoadAsset();
            }
        }
        else  if (!CurrActiveLanguageCode.Equals(languageCode))
        {
            if (IsFileExist())
            {
                if (assetBundle != null)
                {
                    assetBundle.Unload(true);
				}   
                LoadAsset();
            } 
        }
        CurrActiveLanguageCode = languageCode;
       
        return assetBundle;
    }

    void LoadAsset()
    {
        if (assetBundle != null)
        {
            assetBundle.Unload(true);
        }
            
        if (assetBundle == null)
            assetBundle = AssetBundle.LoadFromFile( GetTheDestinationPath() );
    }
	#endregion
   
	public bool IsFileExist() {
        string path = GetTheDestinationPath();
        if (File.Exists(path)) 
		{
			//Debug.Log ("TestingFIleIsThere" + path);
            return true;
        }  
		//Debug.Log ("TestingFIleIsNotThere" + path);
        return false;
    }
	#region CheckFileSize
	public bool IsFileCompletelyDownloaded()
	{
		FileInfo info = new FileInfo ( GetTheDestinationPath ());
		Debug.Log ("TestingFileInfo");
		if (info.Length < UrlApi.instance.sizeOfFile)
			return false;
		return true;	
	}
	#endregion
}


